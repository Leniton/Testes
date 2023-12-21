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
    public float testValue;
    void CalculateParameters()
    {
        //the amount of physics tick per second
        float ticksPerSecond = (1f / Time.fixedDeltaTime)-1;
        //print(ticksPerSecond);

        //offset of missing ticks when calculating gravity
        //at.25: 1.22197525
        //at .5: 1.10093
        //at  1: 1,040522
        //at  2: 1.022412
        //at  3: 1.0007525
        //at  4: 0,99633825
       //dif.25: 0.12104
        //dif.5: 0.060408
        //dif 1: 0.0216595
        //diff roughly 0.03/time
        float tickOffset = ticksPerSecond * Time.fixedDeltaTime * (testValue);
        //Debug.Log(tickOffset);
        //.2== 21
        //.25==18
        //.5== 10
        //.75==6
        //1 == 4
        //2 == 1
        //3 == 0
        //4 == -.5

        //basic speed formula plus the extra force needed to compensate for the gravity force
        jumpSpeed = (jumpHeight / timeToMaxHeight);
        //jumpSpeed *= 1f + ExtraForceWithDrag(jumpSpeed, 1, timeToMaxHeight);
        jumpSpeed *= 1f + testValue;

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
    /// <param name="totalTime">the amount of time the calculation is taking into account</param>
    /// <returns></returns>
    float ExtraForceWithDrag(float speed, float dragForce, float totalTime)
    {
        float dragPercentage = dragForce / speed;
        dragPercentage = (dragForce * totalTime) - (Time.fixedDeltaTime*2);
        Debug.Log(dragPercentage);
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

        Vector2 gravityEffect = physicsHandler.GetVelocity();

        float gravityGuess = 0;
        if (gravityEffect.y > 0)
        {
            //currentGravity = jumpGravity;
            gravityGuess = jumpSpeed / timeToMaxHeight;
            gravityGuess = Mathf.Lerp(0, gravityGuess, Time.fixedDeltaTime);
            if (gravityEffect.y - gravityGuess < 0.00001f) gravityGuess = gravityEffect.y;
            currentGravity = gravityGuess;
        }
        else currentGravity = fallGravity;

        //if (gravityEffect.y > -terminalVelocity)
            gravityEffect.y -= currentGravity;

        physicsHandler.SetVelocity(gravityEffect);

        if(checkStopTime)
        {
            stopTime += Time.fixedDeltaTime;
            speedLost += gravityGuess;
            //Debug.Log($"losing {gravityGuess} from {jumpSpeed}; total: {speedLost}");
        }

        if (gravityEffect.y <= 0)
        {
            if (checkStopTime)
            {
                checkStopTime = false;
                Debug.LogError($"stop time: {stopTime} | height: {transform.position.y - t_initialHeight} \ncurrentSpeed: {gravityEffect.y}");
                Debug.DrawRay(transform.position, Vector3.down * t_initialHeight, Color.red + (Color.yellow / 2), .2f);
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
            t_initialHeight = data.other.bounds.extents.y;
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
