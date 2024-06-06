using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoAbstraction : MonoBehaviour, ICodeAbstraction
{
    public List<ICodeAbstraction> subAbstractions { get; set; }

    public abstract string GetCode();
}
