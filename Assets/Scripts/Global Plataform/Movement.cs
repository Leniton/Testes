using System;
using UnityEngine;

[Serializable]
public class Movement : Displacement
{
    //movement parameters
    [SerializeField] float timeToTopSpeed, speed, timeToStop;
    float accelerationRate, decelerationRate;

    public override void CalculateParameters()
    {
        
    }

    public Vector3 Move(Vector3 direction)
    {
        direction *= speed;
        direction.y = 0;
        return direction;
    }
}
