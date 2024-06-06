using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ItemAbstraction : MonoBehaviour, ICodeAbstraction
{
    public string name { get; set; }
    public List<ICodeAbstraction> subAbstractions { get ; set ; }

    public string GetCode()
    {
        StringBuilder sb = new StringBuilder(name);

        for (int i = 0; i < subAbstractions.Count; i++)
        {
            sb.Append($".{subAbstractions[i].GetCode()}");
        }

        return sb.ToString();
    }
}
