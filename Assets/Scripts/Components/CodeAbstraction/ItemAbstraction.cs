using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ItemAbstraction : ParentAbstraction
{
    [SerializeField] DiceSideAbstraction diceSide;

    private void Awake()
    {
        SetOptions(GeneralDatabase.ItemPickOptions);
    }

    public override string GetCode()
    {
        StringBuilder sb = new StringBuilder(name);

        for (int i = 0; i < subAbstractions.Count; i++)
        {
            sb.Append($".{subAbstractions[i].GetCode()}");
        }

        return sb.ToString();
    }
}
