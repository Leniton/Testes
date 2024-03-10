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
    [SerializeField] private float controlJumpThreshold;
    //Reference Parameters
    [SerializeField] Jump jump;
    [SerializeField] Movement movement;

    private Jump jumpOverride;
    public Jump Jump
    {
        get { return jumpOverride ?? jump; }
        set { jumpOverride = value; }
    }

    Movement movementOverride;
    public Movement Movement
    {
        get { return movementOverride ?? movement; }
        set { movementOverride = value; }
    }

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
        Vector3 xInput = Movement.AdjustToNormal(input, Jump.floorNormal);
        finalVelocity = (finalVelocity * (1 - levelOfControl)) + (Movement.Move(xInput) * levelOfControl);
        finalVelocity.y = physicsHandler.Velocity.y;

        if (levelOfControl >= controlJumpThreshold)
        {
            if (input.y > 0)
            {
                if (Jump.onGround)
                {
                    finalVelocity.y = Jump.JumpValue();
                    input.y = 0;
                }
            }
        }

        if(useGravity) finalVelocity.y -= Jump.GravityForce();

        physicsHandler.Velocity = finalVelocity;

        if (finalVelocity.y < 0)
        {
            input.y = 0;
            state = State.jumping;
        }
    }
}