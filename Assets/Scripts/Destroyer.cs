using System;
using InputSystemHelper;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = InputSystemHelper.Input;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class Destroyer : MonoBehaviour
{
    private Camera mainCamera;
    private bool dragging;
    private Vector2 startPos;

    private void Awake()
    {
        mainCamera = Camera.main;
        var action = Input.Map("Player").Action("Click");
        action.performed += OnClick;
        action.canceled += OnClick;
    }

    private void OnClick(CallbackContext context)
    {
        dragging = context.performed;
        if (dragging) startPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        else Shoot(Mouse.current.position.ReadValue());
    }

    private void DrawRay(Vector2 end)
    {
        Vector2 endPoint = mainCamera.ScreenToWorldPoint(end);
        Debug.DrawRay(startPos, endPoint - startPos, Color.red, .02f);
    }

    private void Shoot(Vector2 finalPosition)
    {
        Vector2 direction = mainCamera.ScreenToWorldPoint(finalPosition) - (Vector3)startPos;
        direction.Normalize();

        var result = Physics2D.Raycast(startPos, direction);
        if (result.collider == null) return;
        if (!result.collider.TryGetComponent<DestructibleObject>(out var destructibleObject)) return;
        Debug.Log("hit destructibleObject");
        destructibleObject.DestroyObject(result.point, direction);
    }

    private void Update()
    {
        if (!dragging) return;
        DrawRay(Mouse.current.position.ReadValue());
    }
}
