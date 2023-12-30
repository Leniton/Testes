using System;
using UnityEngine;

[Serializable]
public class Movement : Displacement
{
    //movement parameters
    [SerializeField] float timeToTopSpeed, topSpeed, timeToStop;
    private float accelerationRate, decelerationRate;
    private float lastUpdateTime = -1;
    private float currentTime = 0;
    private Vector3 lastDirection;

    public override void CalculateParameters()
    {
        accelerationRate = 1;
        decelerationRate = 1f;
    }

    public Vector3 Move(Vector3 direction)
    {
        if (lastUpdateTime < 0) lastUpdateTime = Time.time;
        float elapsedTime = Time.time - lastUpdateTime;
        float modifier;

        if (direction == Vector3.zero)
        {
            modifier = -decelerationRate;
            direction = lastDirection;
        }
        else
        {
            modifier = accelerationRate;
            lastDirection = direction;
        }

        modifier *= elapsedTime;
        currentTime = Mathf.Clamp01(currentTime + modifier);
        float ajustedSpeed = Mathf.Lerp(0, topSpeed, currentTime);
        direction *= ajustedSpeed;

        lastUpdateTime = Time.time;
        if (currentTime <= 0 && modifier < 0) lastUpdateTime = -1;

        return direction;
    }
}
