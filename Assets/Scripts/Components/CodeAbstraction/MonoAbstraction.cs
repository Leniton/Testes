using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoAbstraction : MonoBehaviour, ICodeAbstraction
{
    public string name { get; set; }
    public List<ICodeAbstraction> subAbstractions { get; set; } = new();

    public abstract string GetCode();
}
