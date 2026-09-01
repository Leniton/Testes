using System;
using UnityEngine;
using PhysicsHelper;

public abstract class Displacement<T> : ICopy<T>
{
    protected PhysicsHandler physicsHandler;
    [SerializeField] public Vector3 orientation = Vector3.up;

    public virtual void Initialize(PhysicsHandler handler)
    {
        physicsHandler = handler;
        CalculateParameters();
    }
    public abstract void CalculateParameters();
    public abstract T GetCopy();
}
