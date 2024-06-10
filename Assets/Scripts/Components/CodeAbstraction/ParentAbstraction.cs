using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ParentAbstraction : MonoAbstraction 
{
    [SerializeField] private OptionPicker _optionPick;
    protected Dictionary<string, Func<MonoAbstraction>> options;
    protected OptionPicker OptionPick
    {
        get
        {
            if(!_optionPick) _optionPick = GetComponentInChildren<OptionPicker>();
            return _optionPick;
        }
    }

    protected void SetOptions(Dictionary<string, Func<MonoAbstraction>> dictionary)
    {
        options = dictionary;

        List<string> dropdownOptions = new ();
        foreach (string option in options.Keys) 
        {
            dropdownOptions.Add(option);
        }

        OptionPick.SetOptions(dropdownOptions);
        OptionPick.onPick += PickAbstraction;
    }

    protected void PickAbstraction(int index)
    {
        string key = OptionPick.options[index].Text;

        CreateAbstraction(options[key]);
    }

    protected void CreateAbstraction(Func<MonoAbstraction> factory)
    {
        MonoAbstraction abstraction = factory.Invoke();
        Transform parent = OptionPick.transform.parent;
        abstraction.transform.SetParent(parent, false);
        abstraction.transform.SetSiblingIndex(parent.childCount - 2);
        subAbstractions.Add(abstraction);
    }
}
