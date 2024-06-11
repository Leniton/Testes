using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HeroAbstraction : ParentAbstraction
{
    [SerializeField] private RectTransform typeSlot;
    [SerializeField] private ChoseSidesAbstraction sides;
    MonoAbstraction hero;
    
    protected void Awake()
    {
        name = "";
        SetOptions(GeneralDatabase.HeroPickOptions);
    }
    
    public void SetHero(MonoAbstraction newHero)
    {
        hero = newHero;
        hero.transform.SetParent(typeSlot);
    }

    [SerializableMethods.SerializeMethod]
    public override string GetCode(StringBuilder sb)
    {
        sb ??= new StringBuilder();
        hero.GetCode(sb);
        if (sb.ToString()[0] == '.') sb.Remove(0, 1);
        if(sides.isActive) sb.Append('.');
        sides.GetCode(sb);

        for (int i = 0; i < subAbstractions.Count; i++)
        {
            sb.Append($".");
            subAbstractions[i].GetCode(sb);
        }

        Debug.Log(sb.ToString());
        return sb.ToString();
    }
}
