using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public interface ICodeAbstraction
{
    public string name { get; set; }
    public List<ICodeAbstraction> subAbstractions { get; set; }

    public string GetCode(StringBuilder sb);
}
