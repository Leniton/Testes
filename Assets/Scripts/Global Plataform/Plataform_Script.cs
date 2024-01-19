using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsHandler))]
public class Plataform_Script : MonoBehaviour
{
#if UNITY_EDITOR
    public bool testMode = true;
#endif
    public float levelOfControl = 1;//change to float level of control?(0 no control, 1 full control)
    public bool useGravity = true;
    public enum State { idle, walking, jumping }
    [SerializeField] public State state = new State();

    public Vector3 input = Vector3.zero;

    //Reference Parameters
    [SerializeField] Jump jump;
    [SerializeField] Movement movement;

    PhysicsHandler physicsHandler;

    Vector3 finalVelocity;
    GameObject lastWall;

    void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();
        jump.Init(physicsHandler);
        movement.Init(physicsHandler);
    }

    void FixedUpdate()
    {
#if UNITY_EDITOR
        if (testMode)
        {
            jump.CalculateParameters();
            movement.CalculateParameters();
        }
#endif

        finalVelocity = physicsHandler.Velocity;
        Vector3 xInput = movement.AdjustToNormal(input, jump.floorNormal);
        finalVelocity = (finalVelocity * (1 - levelOfControl)) + (movement.Move(xInput) * levelOfControl);
        finalVelocity.y = physicsHandler.Velocity.y;

        if (levelOfControl >= 1)
        {
            if (input.y > 0)
            {
                if (jump.onGround)
                {
                    finalVelocity.y = jump.JumpValue();
                    input.y = 0;
                }
            }
        }

        if(useGravity) finalVelocity.y -= jump.GravityForce();

        physicsHandler.Velocity = finalVelocity;

        if (finalVelocity.y < 0)
        {
            input.y = 0;
            state = State.jumping;
        }
    }
}