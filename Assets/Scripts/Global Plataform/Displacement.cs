using System;
using UnityEngine;

public abstract class Displacement
{
    protected PhysicsHandler physicsHandler;

    public virtual void Init(PhysicsHandler handler = null) => physicsHandler = handler;
    public abstract void CalculateParameters();
}
