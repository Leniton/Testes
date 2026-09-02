using System.Collections.Generic;
using InputSystemHelper;
using LenixSO.Sequences;
using LenixSO.Sequences.Composite;
using LenixSO.Sequences.Coroutines;
using LenixSO.Sequences.Decorator;
using PhysicsHelper;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = InputSystemHelper.Input;
using Logger = LenixSO.Logger.Logger;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Plataform_Script plataform;
    [SerializeField] private Collider2D aCollider;
    [SerializeField] private Collider2D bCollider;
    
    private ISequence directionSequence;
    private ISequence launchSequence;
    private ISequence moveSequence;
    
    private AppliedForce appliedForce;

    private void Awake()
    {
        directionSequence = new ObserverSequence(new CoroutineSequence(new(() => CoroutineExtensions.DelayCoroutine(.1f))),
                () =>
                {
                    Time.timeScale = .2f;
                    plataform.input = Vector2.zero;
                    appliedForce = plataform.physicsHandler.ApplyForce(Vector3.zero, 0);
                    plataform.useGravity = false;
                    plataform.levelOfControl = 0;
                })
            .AddFinishedCallback(() => Time.timeScale = 1f);
        launchSequence = new CustomSequence(
            () => appliedForce.Force = 10,
            () =>
            {
                float duration = 1f;
                plataform.physicsHandler.RemoveForce(appliedForce, duration);
                CoroutineExtensions.AwaitCoroutine(CoroutineExtensions.DelayCoroutine(duration), () =>
                {
                    plataform.useGravity = true;
                    plataform.levelOfControl = 1;
                });
            });

        moveSequence = new QueuedSequences(directionSequence, launchSequence);
        
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
        if (directionSequence.running)
        {
            if (data != Vector2.zero) appliedForce.Direction = data;
        }
        else plataform.input = data;
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
        // return;
        //slowdown + launch
        plataform.physicsHandler.TriggerExit += OnLeaveCollider;
        moveSequence.Begin();
        return;

        void OnLeaveCollider(ColliderData c)
        {
            Debug.Log("left");
            plataform.physicsHandler.TriggerExit -= OnLeaveCollider;
            collider.isTrigger = false;
            moveSequence.End();
        }
    }
}
