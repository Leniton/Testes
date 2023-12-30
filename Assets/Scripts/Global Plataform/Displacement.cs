using System;
using UnityEngine;

public abstract class Displacement
{
    protected PhysicsHandler physicsHandler;
    [SerializeField] public Vector3 orientation = Vector3.up;

    public virtual void Init(PhysicsHandler handler)
    {
        physicsHandler = handler;
        CalculateParameters();
    }
    public abstract void CalculateParameters();
}
