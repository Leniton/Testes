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
    [SerializeField] float jumpHeight;
    [SerializeField, Min(.2f)] float timeToMaxHeight;
    [SerializeField, Min(.2f)] float timeToFall;
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
        //float dragMultiplier = 1 + (Mathf.Pow((1 - Time.fixedDeltaTime * gravity), (1 / Time.fixedDeltaTime)) * timeToMaxHeight);
        float ticksPerSecond = (1f / Time.fixedDeltaTime);
        //print(ticksPerSecond);

        jumpSpeed = (jumpHeight / timeToMaxHeight);
        jumpSpeed *= 1 + (Mathf.Pow((1f - Time.fixedDeltaTime * (jumpSpeed / (ticksPerSecond * timeToMaxHeight))), ticksPerSecond * timeToMaxHeight));
        //jumpSpeed *= 2;

        jumpGravity = (jumpSpeed / (ticksPerSecond * timeToMaxHeight));
        fallGravity = (jumpSpeed / (ticksPerSecond * timeToFall));

        currentGravity = jumpGravity;

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

    IEnumerator TestCount()
    {
        yield return new WaitForSeconds(timeToMaxHeight);
        print($"End Height:{transform.position.y}");
    }

    //test parameters
    bool checkStopTime;
    float stopTime;
    int ticksCount = 0;
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
            ticksCount++;
        }

        if (gravityEffect.y <= 0)
        {
            if (checkStopTime)
            {
                checkStopTime = false;
                print($"stop time: {stopTime} | height: {transform.position.y}");
                //print(ticksCount);
                stopTime = 0;
                ticksCount = 0;
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
        if (data.collider.gameObject.CompareTag("Chão"))
        {
            standingFloor = data.collider.gameObject;
            onGround = true;
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
