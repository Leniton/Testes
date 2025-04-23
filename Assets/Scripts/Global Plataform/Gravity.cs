using System;
using UnityEngine;

[Serializable]
public class Gravity : Displacement
{
    [SerializeField] private float referenceHeight = 1;
    [SerializeField, Min(.2f)] private float timeToLand;
    private float finalVelocity;

    private float fallGravity;

    public Gravity(float height = 1, float timeToLand = 1, float finalSpeed = 0)
    {
        referenceHeight = height;
        this.timeToLand = timeToLand;
        finalVelocity = finalSpeed;
    }

    public override void CalculateParameters()
    {
        float stepsRequired = TimeSteps(timeToLand);
        float startingSpeed = InitialVelocity(referenceHeight, timeToLand, finalVelocity);
        fallGravity = (startingSpeed / (stepsRequired - 1));

        //Debug.Log($"startingSpeed: {startingSpeed} | gravity: {fallGravity}\nsteps: {stepsRequired} ");
    }

    public float GravityForce()
    {
        return fallGravity;
    }

    public static float TimeSteps(float time) => time / Time.fixedDeltaTime;

    public static float InitialVelocity(float displacement, float time, float finalVelocity)
    {
        float stepsRequired = TimeSteps(time);
        float startingSpeed = ((2 * displacement / stepsRequired) - finalVelocity) / Time.fixedDeltaTime;
        return startingSpeed;
    }
}
