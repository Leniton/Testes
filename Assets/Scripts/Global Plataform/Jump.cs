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
        //initialize self and gravities
        base.Initialize(handler);

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
        //alter height bc the faster the jump the higher the height offset
        //calculate proportion (simplify, from .2 to .1, .07, .048 ...)
        //[time = offset] .2 = .2; .4 = .1; .6 = .07; .8 = .048
        //offset = (.2/(time/.2))

        //this formula can probably be improved
        float overReachOffset = .2f / (timeToMaxHeight / .2f);
        overReachOffset *= .999999f;//slight nerf to make sure it reaches the height 

        //calculate what hight would have a offset that equals the desired height
        //ex: x* (1+.2) = 10 => x = 10/1.2 => x = 8.34
        float adjustedHeight = jumpHeight / (1 + overReachOffset);

        //Debug.Log($"[{overReachOffset}] - {jumpHeight}({jumpHeight * (1 + overReachOffset)}) => {adjustedHeight}({adjustedHeight * (1 + overReachOffset)})");

        //uses the same formula as gravity to keep it consistent
        jumpSpeed = Gravity.InitialVelocity(adjustedHeight, timeToMaxHeight, 0);
        //Debug.Log(jumpSpeed);

        //create gravity
        jumpGravity = new Gravity(adjustedHeight, timeToMaxHeight);
        fallGravity = new Gravity(jumpHeight, timeToLand);
        jumpGravity.Initialize(physicsHandler);
        fallGravity.Initialize(physicsHandler);

        //gravity calculations. jump and fall gravity are different
        jumpGravity.CalculateParameters();
        fallGravity.CalculateParameters();
    }

    public float JumpValue()
    {
        return jumpSpeed;
    }

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
