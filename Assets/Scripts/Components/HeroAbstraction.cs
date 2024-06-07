using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HeroAbstraction : ParentAbstraction
{
    [SerializeField] Sprite[] sprites;
    protected void Awake()
    {
        subAbstractions = new();
        SetOptions(GeneralDatabase.HeroPickOptions);
        Debug.Log(DiceSideDatabase.Blank.sprite);
        //CreateAbstraction(() => GeneralDatabase.Name(""));
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
