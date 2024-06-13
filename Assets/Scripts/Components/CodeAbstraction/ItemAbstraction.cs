using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ItemAbstraction : ParentAbstraction
{
    [SerializeField] private RectTransform typeSlot;
    [SerializeField] DiceSideAbstraction diceSide;
    MonoAbstraction item;

    public override string name { get; set; } = "i";
    public override List<ICodeAbstraction> subAbstractions { get => diceSide.subAbstractions; set => diceSide.subAbstractions = value; }

    private void Awake()
    {
        SetOptions(GeneralDatabase.ItemPickOptions);
    }

    public void SetItem(MonoAbstraction newItem)
    {
        item = newItem;
        newItem.transform.SetParent(typeSlot);
        subAbstractions.Add(item);
    }

    public override string GetCode(StringBuilder sb)
    {
        sb ??= new StringBuilder();
        if(!string.IsNullOrEmpty(name))
        {
            sb.Append(name);
            sb.Append('.');
        }
        diceSide.GetCode(sb);
        Debug.Log(sb.ToString());
        return sb.ToString();
    }
}
