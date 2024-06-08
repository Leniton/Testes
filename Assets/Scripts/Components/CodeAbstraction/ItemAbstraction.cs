using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ItemAbstraction : ParentAbstraction
{
    MonoAbstraction item;
    [SerializeField] DiceSideAbstraction diceSide;
    public override List<ICodeAbstraction> subAbstractions { get => diceSide.subAbstractions; set => diceSide.subAbstractions = value; }

    private void Awake()
    {
        name = "i";
        item = GeneralDatabase.Keyword(Keyword.cantrip);
        item.transform.SetParent(transform.GetChild(0), false);
        ((RectTransform)item.transform).anchoredPosition = Vector3.zero;
        subAbstractions.Add(item);

        SetOptions(GeneralDatabase.ItemPickOptions);
    }

    [SerializableMethods.SerializeMethod]
    public override string GetCode(StringBuilder sb)
    {
        sb = sb ?? new StringBuilder();
        sb.Append(name);
        sb.Append('.');
        diceSide.GetCode(sb);
        Debug.Log(sb.ToString());
        return sb.ToString();
    }
}
