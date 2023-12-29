using System;
using UnityEngine;

public abstract class Displacement
{
    protected PhysicsHandler physicsHandler;

    public virtual void Init(PhysicsHandler handler)
    {
        physicsHandler = handler;
        CalculateParameters();
    }
    public abstract void CalculateParameters();
}
