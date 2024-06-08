using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ItemAbstraction : ParentAbstraction
{
    [SerializeField] DiceSideAbstraction diceSide;
    public override List<ICodeAbstraction> subAbstractions { get => diceSide.subAbstractions; set => diceSide.subAbstractions = value; }

    private void Awake()
    {
        SetOptions(GeneralDatabase.ItemPickOptions);
    }

    [SerializableMethods.SerializeMethod]
    public override string GetCode(StringBuilder sb)
    {
        sb = sb ?? new StringBuilder();
        sb.Append(name);
        diceSide.GetCode(sb);
        /*for (int i = 0; i < subAbstractions.Count; i++)
        {
            sb.Append($".");
            subAbstractions[i].GetCode(sb);
        }*/

        return sb.ToString();
    }
}
