using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAbility : Ability
{
    [SerializeField] float duration;
    [SerializeField] Movement runParameters;

    protected override void TriggerAbility(Vector3 direction)
    {
        runParameters.CalculateParameters(); //test only

        plataformInput.enabled = false;
        plataform.Movement = runParameters;
        direction.Scale(Vector2.right);
        plataform.input = direction;
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        yield return new WaitForSeconds(duration);
        plataform.Movement = null;
        plataform.input = Vector3.zero;
        plataformInput.enabled = true;
    }
}
