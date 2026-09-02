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
                    plataform.useGravity = false;
                    plataform.levelOfControl = 0;
                })
            .AddFinishedCallback(() => Time.timeScale = 1f);
        launchSequence = new CustomSequence(
            () => appliedForce.Force = 10,
            () =>
            {
                float duration = 1f;
                plataform.useGravity = true;
                plataform.levelOfControl = 1;
                plataform.physicsHandler.RemoveForce(appliedForce, duration);
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
            if (data != Vector2.zero) appliedForce.Direction = data;
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
        // collider.isTrigger = true;
        collider.enabled = true;
        List<Collider2D> colliders = new List<Collider2D>();
        var count = Physics2D.OverlapCollider(collider, colliders);
        Collider2D other = null;
        // Logger.Log(count);
        bool insideCollider = false;
        for (int i = 0; i < count; i++)
        {
            if (colliders[i].gameObject.layer != collider.gameObject.layer) continue;
            insideCollider = true;
            other = colliders[i];
            break;
        }
        collider.isTrigger = insideCollider;
        if (!insideCollider) return;
        // return;
        //slowdown + launch
        var exitDirection = Vector3.right;
        float distance = GetDistance(new Vector3(collider.bounds.min.x, collider.bounds.center.y, 0), Vector3.right);//left
        GetSmallerDistance(new Vector3(collider.bounds.center.x, collider.bounds.max.y, 0), Vector3.down);//top
        GetSmallerDistance(new Vector3(collider.bounds.max.x, collider.bounds.center.y, 0), Vector3.left);//right
        GetSmallerDistance(new Vector3(collider.bounds.center.x, collider.bounds.min.y, 0), Vector3.up);//bot
        plataform.physicsHandler.TriggerExit += OnLeaveCollider;
        Debug.Log(exitDirection);
        appliedForce = plataform.physicsHandler.ApplyForce(exitDirection, 0);
        moveSequence.Begin();
        return;

        void GetSmallerDistance(Vector3 origin, Vector3 direction)
        {
            float newDistance = GetDistance(origin, direction);
            // Debug.Log($"{direction} : {newDistance} - {distance}");
            if (newDistance >= distance) return;
            distance = newDistance;
            exitDirection = direction;
        }

        float GetDistance(Vector3 origin, Vector3 direction)
        {
            var results = new List<RaycastHit2D>();
            var hits = Physics2D.Raycast(origin, direction*0.8f, new ContactFilter2D(), results);
            // Debug.Log(hits);
            for (int i = 0; i < hits; i++)
            {
                // Debug.Log(results[i].collider.gameObject.name);
                if (results[i].collider != other) continue;
                Debug.Log($"{direction} | {results[i].distance} | {results[i].normal}");
                Debug.DrawRay(origin, direction, Color.red, 1);
                return results[i].distance;
            }
            return float.MaxValue;
        }

        void OnLeaveCollider(ColliderData c)
        {
            plataform.physicsHandler.TriggerExit -= OnLeaveCollider;
            collider.isTrigger = false;
            moveSequence.End();
        }
    }
}
