using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HeroAbstraction : MonoBehaviour, ICodeAbstraction
{
    public string name { get; set; }
    public List<ICodeAbstraction> subAbstractions { get; set; }

    private void Awake()
    {
        subAbstractions = new();
        TextAbstraction text = GeneralDatabase.Name("name");
        text.transform.SetParent(transform);
        text.transform.SetSiblingIndex(transform.childCount - 2);
        subAbstractions.Add(GeneralDatabase.Name("name"));
    }

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
