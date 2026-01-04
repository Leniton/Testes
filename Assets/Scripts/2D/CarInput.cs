using System;
using System.Collections;
using System.Collections.Generic;
using PhysicsHelper;
using UnityEngine;

[RequireComponent(typeof(CarMovement))]
public class CarInput : MonoBehaviour
{
    [SerializeField] private CarMovement movement;

    // Update is called once per frame
    void Update()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.Space)) input.y = 1;
        else input.y = 0;
        //input.x = Input.GetAxis("Horizontal");
        //if(Input.GetMouseButtonDown(0)) CalculateDirection();
        if(input.y != 0) movement.targetDirection = input.y != 0 ? CalculateDirection() : null;
        movement.input = input;
    }

    private Vector3 CalculateDirection()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        var direction = (mousePosition - transform.position).normalized;
        return direction;
    }

    private void Reset()
    {
        movement = GetComponent<CarMovement>();
    }
}
