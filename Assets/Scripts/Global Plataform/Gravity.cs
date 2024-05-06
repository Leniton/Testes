using System;
using UnityEngine;

[Serializable]
public class Gravity : Displacement
{
    [SerializeField] float referenceHeight;
    [SerializeField, Min(.2f)] float timeToLand;
    float fallGravity;
    float terminalVelocity;

    public override void CalculateParameters()
    {
        fallGravity = (2 * referenceHeight) / (Mathf.Pow(timeToLand, 2));
    }

    public float GravityForce()
    {
        return fallGravity * Time.fixedDeltaTime;
    }
}
