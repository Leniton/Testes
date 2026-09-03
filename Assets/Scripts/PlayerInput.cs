using System.Collections.Generic;
using InputSystemHelper;
using LenixSO.Sequences;
using LenixSO.Sequences.Composite;
using LenixSO.Sequences.Coroutines;
using LenixSO.Sequences.Decorator;
using PhysicsHelper;
using UnityEngine;
using UnityEngine.InputSystem;
using Util.Extensions;
using Input = InputSystemHelper.Input;
using Logger = LenixSO.Logger.Logger;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Plataform_Script plataform;
    [SerializeField] private Collider2D aCollider;
    [SerializeField] private Collider2D bCollider;
    
    private InputAction moveAction;
    
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
        launchSequence = new QueuedSequences(
            new CoroutineSequence(new(()=>ScriptAnimations.Animate(f => appliedForce.Force = Mathf.Lerp(5, 30, f), customDuration: .2f))),
            new CustomSequence(null, () =>
            {
                // Debug.Log(appliedForce.Force);
                float duration = 1f;
                plataform.useGravity = true;
                plataform.levelOfControl = 1;
                plataform.physicsHandler.RemoveForce(appliedForce, duration);
            }));

        moveSequence = new QueuedSequences(directionSequence, launchSequence);
        
        plataform ??= GetComponent<Plataform_Script>();
        if (plataform == null) return;
        moveAction = Input.Map("Player").Action("Move");
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
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
        if (moveSequence.running)
        {
            plataform.physicsHandler.RemoveForce(appliedForce);
            moveSequence.End();
        }
        //slowdown + launch
        var input = moveAction.ReadValue<Vector2>();
        var exitDirection = input;
        float distance = 0;
        if (exitDirection == Vector2.zero)
        {
            // Debug.Log("no input");
            exitDirection = Vector2.right;
            distance = float.MaxValue;
            GetSmallerDistance(new Vector3(collider.bounds.min.x, collider.bounds.center.y, 0), Vector3.right); //left
            GetSmallerDistance(new Vector3(collider.bounds.center.x, collider.bounds.max.y, 0), Vector3.down); //top
            GetSmallerDistance(new Vector3(collider.bounds.max.x, collider.bounds.center.y, 0), Vector3.left); //right
            GetSmallerDistance(new Vector3(collider.bounds.center.x, collider.bounds.min.y, 0), Vector3.up); //bot
        }

        if (distance == float.MaxValue) //player completely inside collider
        {
            // Debug.Log("inside");
            GetSmallerDistance(new Vector3(other.bounds.min.x, collider.bounds.center.y, 0), Vector3.right, collider);//left
            GetSmallerDistance(new Vector3(collider.bounds.center.x, other.bounds.max.y, 0), Vector3.down, collider);//top
            GetSmallerDistance(new Vector3(other.bounds.max.x, collider.bounds.center.y, 0), Vector3.left, collider);//right
            GetSmallerDistance(new Vector3(collider.bounds.center.x, other.bounds.min.y, 0), Vector3.up, collider);//bot
        }
        
        plataform.physicsHandler.TriggerExit += OnLeaveCollider;
        plataform.physicsHandler.CollisionEnter += OnHitOtherCollider;
        // Debug.Log(exitDirection);
        appliedForce = plataform.physicsHandler.ApplyForce(exitDirection, 0);
        moveSequence.Begin();
        return;

        void GetSmallerDistance(Vector3 origin, Vector3 direction, Collider2D target = null)
        {
            target ??= other;
            float newDistance = GetDistance(origin, direction, target);
            // Debug.Log($"{direction} : {newDistance} - {distance}");
            if (newDistance >= distance) return;
            distance = newDistance;
            exitDirection = -direction;
        }

        float GetDistance(Vector3 origin, Vector3 direction, Collider2D target = null)
        {
            target ??= other;
            var results = new List<RaycastHit2D>();
            var hits = Physics2D.Raycast(origin, direction, new ContactFilter2D { useTriggers = true }, results);
            // Debug.Log(hits);
            for (int i = 0; i < hits; i++)
            {
                // Debug.Log($"hit {results[i].collider.name}, looking for {target}");
                if (results[i].collider != target) continue;
                if (results[i].distance <= 0) break;//inside collider
                // Debug.Log($"{results[i].collider.name} => {direction} | {results[i].distance}");
                // Debug.DrawRay(origin, direction, Color.red, 1);
                return results[i].distance;
            }
            return float.MaxValue;
        }

        void RemoveListeners()
        {
            plataform.physicsHandler.TriggerExit -= OnLeaveCollider;
            plataform.physicsHandler.CollisionEnter -= OnHitOtherCollider;
        }
        
        void OnLeaveCollider(ColliderData c)
        {
            RemoveListeners();
            collider.isTrigger = false;
            moveSequence.End();
        }

        void OnHitOtherCollider(CollisionData c)
        {
            if (!moveSequence.running) return;
            RemoveListeners();
            moveSequence.End();
            collider.isTrigger = false;
            PlayerScript.KillPlayer();
        }
    }
}
