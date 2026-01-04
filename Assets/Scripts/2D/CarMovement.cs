using System;
using System.Collections;
using System.Collections.Generic;
using PhysicsHelper;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private PhysicsHandler physicsHandler;

    public Vector2 input;
    public Vector2? targetDirection;
    [SerializeField, Range(0.01f, 1)] private float maxTurnSpeed = .1f;
    [SerializeField] private float grip = .5f;
    [SerializeField] private Movement movement;
    [SerializeField] private Movement turnMovement;

    private Vector2 lastInput;
    
    private void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();
        movement.CalculateParameters();
        turnMovement.CalculateParameters();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float speed = movement.Move(input * Vector2.up).y;
        if (input.y < 0 || (input.y == 0 && lastInput.y < 0)) speed *= .4f;
        if (!targetDirection.HasValue)
            transform.Rotate(Vector3.forward, turnMovement.Move(input * Vector2.left).x);
        else RotateToAngle();
        var finalVelocity = HandleVelocity(speed);
        physicsHandler.Velocity = finalVelocity;
        //Debug.DrawRay(transform.position, finalVelocity, Color.red);
        if (input != Vector2.zero) lastInput = input;
    }

    private Vector3 HandleVelocity(float speed)
    {
        Vector3 movement = speed * transform.up;
        Vector3 currentVelocity = physicsHandler.Velocity;
        var dotProduct = Mathf.Abs(Vector3.Dot(transform.up, currentVelocity.normalized));
        if (Mathf.Approximately(dotProduct, 1)) return movement;
        movement = Vector3.Lerp(currentVelocity, movement, grip);
        //Debug.DrawRay(transform.position, currentVelocity, Color.cyan);
        return movement;
    }

    private void RotateToAngle()
    {
        if(!targetDirection.HasValue) return;
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(Vector3.forward, targetDirection.Value), maxTurnSpeed);
    }
}
