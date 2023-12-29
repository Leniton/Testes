using System;
using UnityEngine;

[Serializable]
public class Movement : Displacement
{
    //movement parameters
    [SerializeField] float timeToTopSpeed, topSpeed, timeToStop;
    float accelerationRate, decelerationRate;

    public override void CalculateParameters()
    {
        
    }

    public Vector3 Move(Vector3 direction)
    {
        direction *= topSpeed;
        direction.y = physicsHandler.GetVelocity().y;
        return direction;
    }
}
