using System;
using UnityEngine;

[Serializable]
public class Gravity : Displacement
{
    [SerializeField] private float referenceHeight = 1;
    [SerializeField, Min(.2f)] private float timeToLand;
    private float fallGravity;
    private float terminalVelocity;

    public override void CalculateParameters()
    {
        fallGravity = (2 * referenceHeight) / (Mathf.Pow(timeToLand, 2));
    }

    public float GravityForce()
    {
        return fallGravity * Time.fixedDeltaTime;
    }
}
