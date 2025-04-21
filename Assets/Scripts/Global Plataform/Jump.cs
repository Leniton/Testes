using System;
using UnityEngine;
using PhysicsHelper;

[Serializable]
public class Jump : Displacement
{
    //Jump parameters
    [SerializeField] private float jumpHeight; //the height the jump will reach
    [SerializeField, Min(.2f)] float timeToMaxHeight; //the time it will take to reach max height
    [SerializeField, Min(.2f)] private float timeToLand;
    [Space]
    [Tooltip("How steep a slope can be to still be considered a floor")]
    [SerializeField] private float maxSlopeAngle;
    private float jumpSpeed;

    private Gravity jumpGravity;    
    private Gravity fallGravity;

    public bool onGround { get; private set; }
    public Vector3 floorNormal {  get; private set; }

    public Action<CollisionData> OnLand;
    private GameObject standingFloor;

    public override void Initialize(PhysicsHandler handler)
    {
        //create gravity
        jumpGravity = new Gravity(jumpHeight, timeToMaxHeight);
        fallGravity = new Gravity(jumpHeight, timeToLand);

        //initialize self and gravities
        base.Initialize(handler);
        jumpGravity.Initialize(handler);
        fallGravity.Initialize(handler);

        //listen to collision events
        physicsHandler.CollisionEnter += CollisionEnter;
        physicsHandler.CollisionExit += CollisionExit;
    }
    public void CopyFloorData(Jump overridenJump)
    {
        onGround = overridenJump.onGround;
        floorNormal = overridenJump.floorNormal;
        standingFloor = overridenJump.standingFloor;

        OnLand += overridenJump.OnLand;
    }

    public override void CalculateParameters()
    {
        //uses the same formula as gravity to keep it consistent
        jumpSpeed = Gravity.InitialVelocity(jumpHeight, timeToMaxHeight, 0);
        //Debug.Log(jumpSpeed);

        //gravity calculations. jump and fall gravity are different
        jumpGravity.CalculateParameters();
        fallGravity.CalculateParameters();
    }



    public float JumpValue()
    {
        return jumpSpeed;
    }

    //test parameters
    bool pause = false;
    bool checkStopTime = false;
    private float stopTime;
    private float speedLost;
    /// <summary>
    /// Must be called on FixedUpdate to work properly
    /// </summary>
    public float GravityForce()
    {
        if (onGround) return 0;

        Vector3 currentVelocity = physicsHandler.Velocity;
        currentVelocity.Scale(orientation);
        float gravityEffect = Vector3.Dot(currentVelocity, orientation);

        Gravity currentGravity = gravityEffect > 0 ? jumpGravity : fallGravity;

        float gravityForce = currentGravity.GravityForce();

        //test only
        stopTime += Time.fixedDeltaTime;
        speedLost += gravityEffect;
        if (checkStopTime)
        {
            speedLost += gravityForce;
        }
        if (gravityEffect <= 0)
        {
            if (!checkStopTime)
            { 
                checkStopTime = true;
                Debug.LogError($"stop time: {stopTime} | height: {physicsHandler.transform.position.y} \nspeed lost: {speedLost}");
                //if(pause) UnityEditor.EditorApplication.isPaused = true;
                //Debug.DrawRay(transform.position, Vector3.down * t_initialHeight, Color.red + (Color.yellow / 2), .2f);
                stopTime = 0;
                speedLost = 0;
            }
        }

        //if (gravityEffect.y > -terminalVelocity)
        return gravityForce;
    }
    private void CollisionEnter(CollisionData data)
    {
        //print($"collided with {data.collider.gameObject.name}");
        if (Vector3.Angle(orientation, data.contacts[0].normal) <= maxSlopeAngle)
        {
            standingFloor = data.collider.gameObject;
            onGround = true;

            floorNormal = data.contacts[0].normal;
            OnLand?.Invoke(data);

            //test only
            Debug.LogError($"it took {stopTime}s to land, at {data.relativeVelocity} speed");
            pause = false;
            checkStopTime = false;
            stopTime = 0;
            speedLost = 0;
        }
    }
    private void CollisionExit(CollisionData data)
    {
        //print($"Exit {data.collider.gameObject.name}");
        if (data.gameObject == standingFloor)
        {
            floorNormal = orientation;
            standingFloor = null;
            onGround = false;
        }
    }
}
