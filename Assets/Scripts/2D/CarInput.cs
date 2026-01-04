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
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement.input = input;
    }

    private void Reset()
    {
        movement = GetComponent<CarMovement>();
    }
}
