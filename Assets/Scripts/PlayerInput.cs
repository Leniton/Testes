using System.Collections.Generic;
using InputSystemHelper;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = InputSystemHelper.Input;
using Logger = LenixSO.Logger.Logger;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Plataform_Script plataform;
    [SerializeField] private Collider2D aCollider;
    [SerializeField] private Collider2D bCollider;

    private void Awake()
    {
        plataform ??= GetComponent<Plataform_Script>();
        if (plataform == null) return;
        Input.Map("Player").Action("Move").performed += OnMove;
        Input.Map("Player").Action("Move").canceled += OnMove;
        Input.Map("Player").Action("Jump").canceled += OnSwitch;
        OnSwitch(new());
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        var data = obj.ReadValue<Vector2>();
        plataform.input = data;
    }
    
    private void OnSwitch(InputAction.CallbackContext obj)
    {
        if (bCollider.enabled)
        {
            bCollider.isTrigger = false;
            bCollider.enabled = false;
            EnableCollision(aCollider);
        }
        else
        {
            aCollider.isTrigger = false;
            aCollider.enabled = false;
            EnableCollision(bCollider);
        }
    }

    private void EnableCollision(Collider2D collider)
    {
        collider.isTrigger = true;
        collider.enabled = true;
        List<Collider2D> colliders = new List<Collider2D>();
        var count = Physics2D.OverlapCollider(collider, colliders);
        // Logger.Log(count);
        bool insideCollider = false;
        for (int i = 0; i < count; i++)
        {
            if (colliders[i].gameObject.layer != collider.gameObject.layer) continue;
            insideCollider = true;
            break;
        }
        collider.isTrigger = insideCollider;
        if (!insideCollider) return;
        //slowdown + launch
    }
}
