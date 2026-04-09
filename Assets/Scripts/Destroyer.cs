using System;
using InputSystemHelper;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = InputSystemHelper.Input;

public class Destroyer : MonoBehaviour
{
    private bool dragging;
    private Vector2 startPos;

    private void Awake()
    {
        var action = Input.Map("Player").Action("Click");
        action.performed += OnClick;
        action.canceled += OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        dragging = context.performed;
        if (dragging) startPos = Mouse.current.position.ReadValue();
    }

    private void Update()
    {
        if (!dragging) return;
    }
}
