using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class MonoAbstraction : MonoBehaviour, ICodeAbstraction
{
    public virtual string name { get; set; }
    public virtual List<ICodeAbstraction> subAbstractions { get; set; } = new();

    public abstract string GetCode(StringBuilder sb);
}
