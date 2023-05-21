using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsHandler))]
public class Plataform_Script : MonoBehaviour
{
#if UNITY_EDITOR
    public bool testMode = true;
#endif

    public enum State { idle, walking, jumping }
    [SerializeField] public State state = new State();

    public Vector3 input = Vector3.zero;
    public bool hasControl = true;
    [SerializeField] bool onGround;

    //Reference Parameters
    PhysicsHandler physicsHandler;
    GameObject standingFloor;

    //Jump parameters
    [SerializeField] float jumpHeight; //the height the jump will reach
    [SerializeField, Min(.2f)] float timeToMaxHeight; //the time it will take to reach max height
    [SerializeField, Min(.2f)] float timeToFall; //the time it will take to get to the ground from max jump height
    float jumpSpeed;
    float fallGravity,jumpGravity;
    float currentGravity;
    float terminalVelocity;

    //Movement parameters
    Vector3 finalVelocity;
    float maxSlopeAngle;

    void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();

        //adding events
        physicsHandler.CollisionEnter = CollisionEnter;
        physicsHandler.CollisionExit = CollisionExit;

        CalculateParameters();
    }

    void CalculateParameters()
    {
        //the amount of physics tick per second
        float ticksPerSecond = (1f / Time.fixedDeltaTime)-1;
        //print(ticksPerSecond);

        //offset of missing ticks when calculating gravity
        float tickOffset = ticksPerSecond * Time.fixedDeltaTime*5;
        //basic speed formula plus the extra force needed to compensate for the gravity force
        jumpSpeed = (jumpHeight / timeToMaxHeight);
        jumpSpeed *= 1f + ExtraForceWithDrag(jumpSpeed, (jumpSpeed / (ticksPerSecond * timeToMaxHeight)),
            ticksPerSecond - tickOffset, timeToMaxHeight);

        //gravity calculations. jump and fall gravity are different
        jumpGravity = (jumpSpeed / (ticksPerSecond * timeToMaxHeight));
        fallGravity = (jumpSpeed / (ticksPerSecond * timeToFall));

        currentGravity = jumpGravity;

        //print($"jump: {jumpSpeed}");
        //print($"gravity: {gravity}");
    }

    /// <summary>
    /// Calculates the percentage of force needed to compensate some dictated drag force
    /// </summary>
    /// <param name="speed">the speed you want to calculate the compensation</param>
    /// <param name="dragForce">the force slowing down the speed</param>
    /// <param name="ticksPerSecond">the amount of ticks the drag is added per second</param>
    /// <param name="totalTime">the amount of time the calculation is taking into account</param>
    /// <returns></returns>
    float ExtraForceWithDrag(float speed, float dragForce, float ticksPerSecond,float totalTime)
    {
        float dragPercentage = dragForce / speed;
        dragPercentage *= (ticksPerSecond * totalTime);

        return dragPercentage;
    }

    void FixedUpdate()
    {

#if UNITY_EDITOR
        if (testMode)
        {
            CalculateParameters();
        }
#endif

        if (hasControl)
        {
            if(input.y > 0)
            {
                if (onGround)
                {
                    checkStopTime = true;
                    stopTime = 0;
                    Jump();
                    //print($"Start Height:{transform.position.y}");
                    //StartCoroutine(TestCount());
                    input.y = 0;
                    physicsHandler.SetVelocity(finalVelocity);
                }
            }
        }

        Gravity();
    }

    float t_initialHeight;

    //test parameters
    bool checkStopTime;
    float stopTime;
    float speedLost = 0;
    void Gravity()
    {
        if (onGround) return;

        /*if (puloCurto && fallSpot > 0 && !jump && RB.velocity.y > 0 &&
            RB.velocity.y <= pulo * Mathf.Abs(fallSpot - 1))
        {
            Vector2 dropOff = Vector2.one;
            dropOff.y = .2f;
            RB.velocity *= dropOff;
        }*/

        Vector2 gravityEffect = physicsHandler.GetVelocity();

        if (gravityEffect.y >= 0) currentGravity = jumpGravity;
        else currentGravity = fallGravity;

        //if (gravityEffect.y > -terminalVelocity)
            gravityEffect.y -= currentGravity;

        physicsHandler.SetVelocity(gravityEffect);

        if(checkStopTime)
        {
            stopTime += Time.fixedDeltaTime;
            speedLost += jumpSpeed - physicsHandler.GetVelocity().y;
            //Debug.Log($"lost {jumpSpeed - physicsHandler.GetVelocity().y} speed");
        }

        if (gravityEffect.y <= 0)
        {
            if (checkStopTime)
            {
                checkStopTime = false;
                print($"stop time: {stopTime} | height: {transform.position.y-t_initialHeight}");
                //print($"Initial speed: {jumpSpeed} | total speed lost: {speedLost}");
                //float ticksPerSecond = (1f / Time.fixedDeltaTime) - 1;
                //print($"Percentage lost: {ExtraForceWithDrag(jumpSpeed, jumpGravity, ticksPerSecond-1, timeToMaxHeight):0.0000}");
                stopTime = 0;
                speedLost = 0;
            }
        }

        if (gravityEffect.y < 0)
        {
            input.y = 0;
            state = State.jumping;
        }
    }

    void Jump()
    {
        //print($"jump: {jumpSpeed}");
        finalVelocity.y = jumpSpeed;
    }
    void CollisionEnter(CollisionData data)
    {
        //print($"collided with {data.collider.gameObject.name}");
        if (data.collider.gameObject.CompareTag("Ch�o"))
        {
            standingFloor = data.collider.gameObject;
            onGround = true;

            //test only
            t_initialHeight = transform.position.y;
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

    #region Enable/Disable

    private void OnDisable()
    {
        physicsHandler.CollisionEnter = null;
        physicsHandler.CollisionExit = null;
    }

    #endregion
}
