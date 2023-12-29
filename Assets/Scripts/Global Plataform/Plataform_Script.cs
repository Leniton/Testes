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

    //Reference Parameters
    PhysicsHandler physicsHandler;

    [SerializeField] Jump jump;
    [SerializeField] Movement movement;

    //Movement parameters
    Vector3 finalVelocity;

    void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();

        jump.Init(physicsHandler);
        jump.CalculateParameters();
    }

    void FixedUpdate()
    {

#if UNITY_EDITOR
        if (testMode)
        {
            jump.CalculateParameters();
        }
#endif

        finalVelocity = physicsHandler.GetVelocity();
        if (input.y > 0)
        {
            if (jump.onGround)
            {
                finalVelocity.y = jump.JumpValue();
                input.y = 0;
            }
        }
        Vector3 moveValue = movement.Move(input);
        finalVelocity.x = moveValue.x;
        finalVelocity.z = moveValue.z;

        finalVelocity.y -= jump.GravityForce();

        physicsHandler.SetVelocity(finalVelocity);


        if (finalVelocity.y < 0)
        {
            input.y = 0;
            state = State.jumping;
        }
    }
}
