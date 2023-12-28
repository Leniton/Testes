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
    public bool onGround { get; private set; }

    //Reference Parameters
    PhysicsHandler physicsHandler;
    GameObject standingFloor;

    //Jump parameters
    [SerializeField] float jumpHeight; //the height the jump will reach
    [SerializeField, Min(.2f)] float timeToMaxHeight; //the time it will take to reach max height
    [SerializeField, Min(.2f)] float timeToFall; //the time it will take to get to the ground from max jump height
    float jumpSpeed;
    float fallGravity;
    public float gravityCompensation;
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
        //basic speed formula plus the extra force needed to compensate for the gravity force
        jumpSpeed = jumpHeight;
        jumpSpeed *= 1f + gravityCompensation;

        //gravity calculations. jump and fall gravity are different
        fallGravity = (2 * jumpHeight) / (Mathf.Pow(timeToFall, 2));

        //print($"jump: {jumpSpeed}");
        //print($"gravity: {gravity}");
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
                    stopTime = 0;
                    Jump();
                    input.y = 0;
                    physicsHandler.SetVelocity(finalVelocity);
                }
            }
        }

        Gravity();
    }

    //test parameters
    bool checkStopTime = false;
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
        else
        {
            gravityGuess = fallGravity * Time.fixedDeltaTime;
            currentGravity = gravityGuess;
        }

        //if (gravityEffect.y > -terminalVelocity)
            gravityEffect.y -= currentGravity;

        physicsHandler.SetVelocity(gravityEffect);

        if(checkStopTime)
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

        if (gravityEffect.y < 0)
        {
            input.y = 0;
            state = State.jumping;
        }
    }

    void Jump()
    {
        finalVelocity.y = jumpSpeed; 
        checkStopTime = false;
    }

    void CollisionEnter(CollisionData data)
    {
        //print($"collided with {data.collider.gameObject.name}");
        if (data.collider.gameObject.CompareTag("Chão"))
        {
            standingFloor = data.collider.gameObject;
            onGround = true;

            //test only

            Debug.LogWarning($"it took {stopTime}s to land, at {data.relativeVelocity} speed");
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

    #region Enable/Disable

    private void OnDisable()
    {
        physicsHandler.CollisionEnter = null;
        physicsHandler.CollisionExit = null;
    }

    #endregion
}
