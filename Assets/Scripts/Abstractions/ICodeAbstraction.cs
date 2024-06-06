using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICodeAbstraction
{
    public string name { get; set; }
    public List<ICodeAbstraction> subAbstractions { get; set; }

    public string GetCode();
}
