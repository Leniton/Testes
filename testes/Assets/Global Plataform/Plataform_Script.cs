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
    bool onGround;

    //Reference Parameters
    PhysicsHandler physicsHandler;

    //Jump parameters
    [SerializeField] float jumpHeight;
    [SerializeField] float timeToMaxHeight;
    [SerializeField] float timeToFall;
    float jumpSpeed;
    float gravity,jumpGravity;
    float terminalVelocity;

    //Movement parameters
    Vector3 finalVelocity;

    void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();
        
        CalculateParameters();
    }

    void CalculateParameters()
    {
        //float dragMultiplier = 1 + (Mathf.Pow((1 - Time.fixedDeltaTime * gravity), (1 / Time.fixedDeltaTime)) * timeToMaxHeight);
        float ticksPerSecond = (1 / Time.fixedDeltaTime)-1;
        //print(ticksPerSecond);

        jumpSpeed = jumpHeight / (timeToMaxHeight * (1-Time.fixedDeltaTime));
        jumpSpeed += (jumpSpeed / (ticksPerSecond * timeToMaxHeight));
        jumpSpeed *= 2;

        gravity = (jumpSpeed / (ticksPerSecond * timeToMaxHeight));

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
                checkStopTime = true;
                stopTime = 0;
                Jump();
                input.y = 0;
                physicsHandler.SetVelocity(finalVelocity);
            }
        }

        Gravity();
    }

    //test parameters
    bool checkStopTime;
    float stopTime;
    void Gravity()
    {
        //if (onGround) return;

        /*if (puloCurto && fallSpot > 0 && !jump && RB.velocity.y > 0 &&
            RB.velocity.y <= pulo * Mathf.Abs(fallSpot - 1))
        {
            Vector2 dropOff = Vector2.one;
            dropOff.y = .2f;
            RB.velocity *= dropOff;
        }*/

        Vector2 gravityEffect = physicsHandler.GetVelocity();
        //if (gravityEffect.y > -terminalVelocity)
            gravityEffect.y -= gravity;

        physicsHandler.SetVelocity(gravityEffect);

        if(checkStopTime)
        {
            stopTime += Time.fixedDeltaTime;
        }

        if (gravityEffect.y <= 0)
        {
            if (checkStopTime)
            {
                checkStopTime = false;
                print($"stop time: {stopTime}");
                stopTime = 0;
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
        //print(gravity);
        //print(jumpSpeed);

        finalVelocity.y = jumpSpeed;
    }
}
