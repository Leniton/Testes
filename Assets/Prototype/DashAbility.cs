using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class DashAbility : Ability
{
    [SerializeField] Dash dash = new();

    protected override void Awake()
    {
        base.Awake();
        dash.Init(physicsHandler);
        dash.doneDashing += Recover;
    }

    protected override void TriggerAbility(Vector3 direction)
    {
        if (dash.dashing) return;
        dash.CalculateParameters(); //test only
        plataform.levelOfControl = 0;
        plataform.useGravity = false;
        dash.StartDash(direction);
    }

    public void Recover(float time)
    {
        plataform.useGravity = true;
        StartCoroutine(RecoverControl(time));
    }

    IEnumerator RecoverControl(float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            yield return null;
            currentTime += Time.deltaTime;
            plataform.levelOfControl = currentTime / time;
        }

        plataform.levelOfControl = 1;
    }
}
