using System;
using UnityEngine;

[Serializable]
public class Jump : Displacement
{
    //Jump parameters
    [SerializeField] float jumpHeight; //the height the jump will reach
    [SerializeField, Min(.2f)] float timeToMaxHeight; //the time it will take to reach max height
    [SerializeField, Min(.2f)] float timeToFall; //the time it will take to get to the ground from max jump height
    [SerializeField] float gravityCompensation;
    [SerializeField] float maxSlopeAngle;
    float jumpSpeed;
    float fallGravity;
    float terminalVelocity;
    public bool onGround { get; private set; }

    public Action OnLand;
    GameObject standingFloor;

    public override void Init(PhysicsHandler handler)
    {
        base.Init(handler);
        physicsHandler.CollisionEnter += CollisionEnter;
        physicsHandler.CollisionExit += CollisionExit;
    }

    public override void CalculateParameters()
    {
        //basic speed formula plus the extra force needed to compensate for the gravity force
        jumpSpeed = jumpHeight;
        jumpSpeed *= 1f + gravityCompensation;

        //gravity calculations. jump and fall gravity are different
        fallGravity = (2 * jumpHeight) / (Mathf.Pow(timeToFall, 2));

        //print($"jump: {jumpSpeed}");
        //print($"gravity: {gravity}");
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
        stopTime = 0;

        Vector2 gravityEffect = physicsHandler.GetVelocity();

        float currentGravity;
        float gravityGuess;
        if (gravityEffect.y > 0)
        {
            //currentGravity = jumpGravity;
            gravityGuess = jumpSpeed / timeToMaxHeight;
            gravityGuess = Mathf.Lerp(0, gravityGuess, Time.fixedDeltaTime);
            if (gravityEffect.y - gravityGuess < 0.00001f) gravityGuess = gravityEffect.y;
            currentGravity = gravityGuess;
        }
        else
        {
            gravityGuess = fallGravity * Time.fixedDeltaTime;
            currentGravity = gravityGuess;
        }

        //test only
        if (checkStopTime)
        {
            stopTime += Time.fixedDeltaTime;
            speedLost += currentGravity;
            //Debug.Log($"losing {gravityGuess} from {jumpSpeed}; total: {speedLost}");
        }
        if (gravityEffect.y <= 0)
        {
            if (!checkStopTime)
            { 
                //Debug.LogError($"fall acceleration is {fallGravity} per second");
                checkStopTime = true;
                //Debug.LogError($"stop time: {stopTime} | height: {transform.position.y} \ncurrentSpeed: {gravityEffect.y}");
                //Debug.DrawRay(transform.position, Vector3.down * t_initialHeight, Color.red + (Color.yellow / 2), .2f);
                //print($"Initial speed: {jumpSpeed} | total speed lost: {speedLost}");
                //float ticksPerSecond = (1f / Time.fixedDeltaTime) - 1;
                //print($"Percentage lost: {ExtraForceWithDrag(jumpSpeed, jumpGravity, ticksPerSecond-1, timeToMaxHeight):0.0000}");
                stopTime = 0;
                speedLost = 0;
            }
        }

        //if (gravityEffect.y > -terminalVelocity)
        return currentGravity;
    }
    void CollisionEnter(CollisionData data)
    {
        //print($"collided with {data.collider.gameObject.name}");
        if (data.collider.gameObject.CompareTag("Chão") && data.contacts[0].normal.y > 0)
        {
            standingFloor = data.collider.gameObject;
            onGround = true;

            //test only
            //Debug.LogWarning($"it took {stopTime}s to land, at {data.relativeVelocity} speed");
            checkStopTime = false;
            stopTime = 0;
            speedLost = 0;
        }
    }
    void CollisionExit(CollisionData data)
    {
        //print($"Exit {data.collider.gameObject.name}");
        if (data.collider.gameObject == standingFloor)
        {
            standingFloor = null;
            onGround = false;
        }
    }
}
