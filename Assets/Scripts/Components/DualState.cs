using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DualState : MonoBehaviour
{
    [SerializeField] protected bool StartingState;

    protected virtual void Awake()
    {
        SetState(StartingState);
    }

    public abstract void SetState(bool state);
}
