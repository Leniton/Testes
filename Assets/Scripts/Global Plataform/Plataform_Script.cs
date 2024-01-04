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
    [SerializeField] Jump jump;
    [SerializeField] Movement movement;

    PhysicsHandler physicsHandler;

    Vector3 finalVelocity;
    bool canWalkRight = true, canWalkLeft = true;
    GameObject lastWall;

    void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();
        physicsHandler.CollisionEnter += CollisionEnter;
        physicsHandler.CollisionExit += CollisionExit;
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
        Vector3 xInput = movement.AdjustToNormal(input, jump.floorNormal);
        if (xInput.x > 0 && !canWalkLeft) xInput.x = 0;
        if (xInput.x < 0 && !canWalkRight) xInput.x = 0;

        finalVelocity = movement.Move(xInput);
        finalVelocity.y = physicsHandler.Velocity.y;
        if (input.y > 0)
        {
            if (jump.onGround)
            {
                finalVelocity.y = jump.JumpValue();
                input.y = 0;
            }
        }

        finalVelocity.y -= jump.GravityForce();

        physicsHandler.Velocity = finalVelocity;

        if (finalVelocity.y < 0)
        {
            input.y = 0;
            state = State.jumping;
        }
    }

    void CollisionEnter(CollisionData data)
    {
        Vector3 hitNormal = data.contacts[0].normal;
        if (hitNormal.y <= 0)
        {
            //Debug.Log("wall hit");
            if (hitNormal.x < 0)
            {
                canWalkLeft = false;
            }
            else
            {
                canWalkRight = false;
            }
            lastWall = data.gameObject;
        }
    }
    void CollisionExit(CollisionData data)
    {
        if (lastWall == data.gameObject)
        {
            canWalkLeft = true;
            canWalkRight = true;
            lastWall = null;
        }
    }
}