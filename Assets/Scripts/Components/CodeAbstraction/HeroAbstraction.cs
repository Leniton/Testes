using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HeroAbstraction : ParentAbstraction
{
    protected void Awake()
    {
        subAbstractions = new();
        SetOptions(GeneralDatabase.HeroPickOptions);
    }

    [SerializableMethods.SerializeMethod]
    public override string GetCode(StringBuilder sb = null)
    {
        sb = sb ?? new StringBuilder();
        sb.Append(name);

        for (int i = 0; i < subAbstractions.Count; i++)
        {
            sb.Append($".");
            subAbstractions[i].GetCode(sb);
        }

        return sb.ToString();
    }
}
