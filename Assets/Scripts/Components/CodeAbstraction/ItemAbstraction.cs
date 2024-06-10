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
    public override List<ICodeAbstraction> subAbstractions { get => diceSide.subAbstractions; set => diceSide.subAbstractions = value; }

    private void Awake()
    {
        SetOptions(GeneralDatabase.ItemPickOptions);
    }

    public void SetItem(MonoAbstraction newItem)
    {
        item = newItem;
        name = "i";
        newItem.transform.SetParent(typeSlot);
        subAbstractions.Add(item);
    }

    [SerializableMethods.SerializeMethod]
    public override string GetCode(StringBuilder sb)
    {
        sb ??= new StringBuilder();
        sb.Append(name);
        sb.Append('.');
        diceSide.GetCode(sb);
        Debug.Log(sb.ToString());
        return sb.ToString();
    }
}
