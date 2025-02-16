using System;
using UnityEngine;
using PhysicsHelper;

[Serializable]
public class Jump : Displacement
{
    //Jump parameters
    [SerializeField] private float jumpHeight; //the height the jump will reach
    [SerializeField, Min(.05f)] float timeToMaxHeight; //the time it will take to reach max height
    [SerializeField, Min(.05f)] private float timeToLand;
    [Space]
    [SerializeField] private float maxSlopeAngle;
    private float jumpSpeed;

    private Gravity jumpGravity;    
    private Gravity fallGravity;

    public bool onGround { get; private set; }
    public Vector3 floorNormal {  get; private set; }

    public Action<CollisionData> OnLand;
    private GameObject standingFloor;

    public override void Init(PhysicsHandler handler)
    {
        //create gravity
        jumpGravity = new Gravity(jumpHeight, timeToMaxHeight, 0);
        fallGravity = new Gravity(jumpHeight, timeToLand);

        //initialize self and gravities
        base.Init(handler);
        jumpGravity.Init(handler);
        //fallGravity.Init(handler);

        //listen to collision events
        physicsHandler.CollisionEnter += CollisionEnter;
        physicsHandler.CollisionExit += CollisionExit;
    }

    public override void CalculateParameters()
    {
        //uses the same formula as gravity to keep it consistent
        jumpSpeed = Gravity.InitialVelocity(jumpHeight, timeToMaxHeight, 0);
        //Debug.Log(jumpSpeed);

        //gravity calculations. jump and fall gravity are different
        jumpGravity.CalculateParameters();
        //fallGravity.CalculateParameters();
    }



    public float JumpValue()
    {
        return jumpSpeed;
    }

    //test parameters
    bool checkStopTime = false;
    float stopTime;
    float speedLost;
    /// <summary>
    /// Must be called on FixedUpdate to work properly
    /// </summary>
    public float GravityForce()
    {
        if (onGround) return 0;

        //test only

        Vector3 currentVelocity = physicsHandler.Velocity;
        currentVelocity.Scale(orientation);
        float gravityEffect = Vector3.Dot(currentVelocity, orientation);

        Gravity currentGravity = jumpGravity;// gravityEffect > 0 ? jumpGravity : fallGravity;

        float gravityForce = currentGravity.GravityForce();
        //float gravityGuess;
        //if (gravityEffect.y > 0)
        //{
        //    //currentGravity = jumpGravity;
        //    gravityGuess = jumpSpeed / timeToMaxHeight;
        //    gravityGuess = Mathf.Lerp(0, gravityGuess, Time.fixedDeltaTime);
        //    if (gravityEffect.y - gravityGuess < 0.00001f) gravityGuess = gravityEffect.y;
        //    gravityForce = gravityGuess;
        //}
        //else
        //{
        //    gravityForce = fallGravity.GravityForce();
        //}

        //test only
        stopTime += Time.fixedDeltaTime;
        if (checkStopTime)
        {
            speedLost += gravityForce;
            //Debug.Log($"losing {gravityGuess} from {jumpSpeed}; total: {speedLost}");
        }
        if (gravityEffect <= 0)
        {
            if (!checkStopTime)
            { 
                //Debug.LogError($"fall acceleration is {fallGravity} per second");
                checkStopTime = true;
                Debug.LogError($"stop time: {stopTime} | height: {physicsHandler.transform.position.y} \ncurrentSpeed: {gravityEffect}");
                //Debug.DrawRay(transform.position, Vector3.down * t_initialHeight, Color.red + (Color.yellow / 2), .2f);
                //print($"Initial speed: {jumpSpeed} | total speed lost: {speedLost}");
                //float ticksPerSecond = (1f / Time.fixedDeltaTime) - 1;
                //print($"Percentage lost: {ExtraForceWithDrag(jumpSpeed, jumpGravity, ticksPerSecond-1, timeToMaxHeight):0.0000}");
                stopTime = 0;
                speedLost = 0;
            }
        }

        //if (gravityEffect.y > -terminalVelocity)
        return gravityForce;
    }
    void CollisionEnter(CollisionData data)
    {
        //print($"collided with {data.collider.gameObject.name}");
        if (data.gameObject.CompareTag("Chão") && Vector3.Angle(orientation, data.contacts[0].normal) <= maxSlopeAngle)
        {
            standingFloor = data.collider.gameObject;
            onGround = true;

            floorNormal = data.contacts[0].normal;
            OnLand?.Invoke(data);

            //test only
            Debug.LogError($"it took {stopTime}s to land, at {data.relativeVelocity} speed");
            checkStopTime = false;
            stopTime = 0;
            speedLost = 0;
        }
    }
    void CollisionExit(CollisionData data)
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
