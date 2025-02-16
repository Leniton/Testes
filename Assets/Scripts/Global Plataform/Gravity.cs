using System;
using UnityEngine;

[Serializable]
public class Gravity : Displacement
{
    [SerializeField] private float referenceHeight = 1;
    [SerializeField, Min(.2f)] private float timeToLand;
    private float finalVelocity;

    private float fallGravity;

    public Gravity(float height = 1, float timeToLand = 1, float? finalSpeed = null)
    {
        referenceHeight = height;
        this.timeToLand = timeToLand;
        //if no final speed is set, calculate assuming initialVelocity is 0 
        finalVelocity = finalSpeed ??= InitialVelocity(height, timeToLand, 0);
    }

    public override void CalculateParameters()
    {
        //fallGravity = (2 * referenceHeight) / (Mathf.Pow(timeToLand, 2));
        float stepsRequired = TimeSteps(timeToLand);
        float startingSpeed = InitialVelocity(referenceHeight, timeToLand, finalVelocity);
        //float startingSpeed = (2 * referenceHeight / timeToLand) - finalVelocity;
        fallGravity = (startingSpeed / (stepsRequired - 1));

        Debug.Log($"steps: {stepsRequired} | startingSpeed: {startingSpeed}\n gravity: {fallGravity}");
    }

    public float GravityForce()
    {
        return fallGravity;
    }

    public static float TimeSteps(float time) => time / Time.fixedDeltaTime;

    public static float InitialVelocity(float displacement, float time, float finalVelocity)
    {
        float stepsRequired = TimeSteps(time);
        float startingSpeed = (2 * displacement / stepsRequired) - finalVelocity;
        return startingSpeed;
    }
}
