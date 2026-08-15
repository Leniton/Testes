using System;
using InputSystemHelper;
using UnityEngine;
using Input = InputSystemHelper.Input;

[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Movement movement;
    
    private void Awake()
    {
        movement ??= GetComponent<Movement>();
        var move = Input.Map("Player").Action("Move");
        move.performed += context =>
        {
            movement.input = context.ReadValue<Vector2>();
            movement.MoveNow();
        };
        move.canceled += _ => movement.input = Vector2.zero;
    }
}
