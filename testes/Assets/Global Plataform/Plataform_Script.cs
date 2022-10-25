using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsHandler))]
public class Plataform_Script : MonoBehaviour
{
#if UNITY_EDITOR
    bool testMode = true;
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
    float jumpStrength;
    float gravity;
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
        float totalTicks = (1 / Time.fixedDeltaTime);
        print(totalTicks);

        jumpStrength = (jumpHeight / timeToMaxHeight);
        gravity = ((jumpHeight + (jumpStrength * timeToMaxHeight) - jumpHeight) / (Mathf.Pow(timeToMaxHeight, 2) / 2)) / totalTicks;

    }


    void FixedUpdate()
    {
        if (hasControl)
        {
            if(input.y > 0)
            {
                Jump();
                input.y = 0;
                physicsHandler.SetVelocity(finalVelocity);
            }
        }

        Gravity();
    }

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

        if (gravityEffect.y < 0)
        {
            input.y = 0;
            state = State.jumping;
        }
    }

    void Jump()
    {
        print(gravity);
        print(jumpStrength);

        finalVelocity.y = jumpStrength;
    }
}
