using System;
using UnityEngine;
using PhysicsHelper;

public abstract class Displacement
{
    protected PhysicsHandler physicsHandler;
    [SerializeField] public Vector3 orientation = Vector3.up;

    public virtual void Initialize(PhysicsHandler handler)
    {
        physicsHandler = handler;
        CalculateParameters();
    }
    public abstract void CalculateParameters();
}
