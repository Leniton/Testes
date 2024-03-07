using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dash : Displacement
{
    [SerializeField] float distance = .2f;
    [SerializeField] float duration = .2f;
    [SerializeField] bool saveVelocity, keepMomentum;
    public bool dashing;
    float dashSpeed;
    Coroutine coroutine;
    MonoBehaviour mono => physicsHandler;

    public Action doneDashing;

    public override void Init(PhysicsHandler handler)
    {
        base.Init(handler);
        handler.CollisionEnter += CollisionEnter;
    }

    public override void CalculateParameters()
    {
        dashSpeed = distance / duration;
    }

    public void StartDash(Vector3 direction)
    {
        if (dashing) return;
        dashing = true;

        coroutine = mono.StartCoroutine(BeginDash(direction));
    }

    IEnumerator BeginDash(Vector3 direction)
    {
        float time = 0;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        Vector3 currentVelocity = physicsHandler.Velocity;
        Vector3 dashForce = direction * dashSpeed;
        physicsHandler.Velocity = dashForce;

        while (time < duration)
        {
            yield return wait;
            time += Time.fixedDeltaTime;
            physicsHandler.Velocity = dashForce;
        }

        Vector3 finalVelocity = saveVelocity ? currentVelocity : Vector3.zero;
        direction.x = Mathf.Abs(direction.x);
        direction.y = Mathf.Abs(direction.y);
        finalVelocity.Scale(direction);
        if (keepMomentum) finalVelocity += dashForce;
        physicsHandler.Velocity = finalVelocity;

        Debug.Log($"start: {currentVelocity} | end: {finalVelocity}");

        StopDash();
    }

    public void StopDash()
    {
        if (!dashing) return;
        dashing = false;
        mono.StopCoroutine(coroutine);
        coroutine = null;
        doneDashing?.Invoke();
    }

    private void CollisionEnter(CollisionData data)
    {
        StopDash();
    }
}
