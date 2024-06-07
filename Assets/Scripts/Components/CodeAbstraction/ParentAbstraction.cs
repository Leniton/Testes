using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ParentAbstraction : MonoAbstraction 
{
    [SerializeField] private OptionPicker _dropdown;
    protected Dictionary<string, Func<MonoAbstraction>> options;
    protected OptionPicker Dropdown
    {
        get
        {
            if(!_dropdown) _dropdown = GetComponentInChildren<OptionPicker>();
            return _dropdown;
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

        Dropdown.SetOptions(dropdownOptions);
        Dropdown.onPick += PickAbstraction;
    }

    protected void PickAbstraction(int index)
    {
        string key = Dropdown.options[index].Text;

        CreateAbstraction(options[key]);
    }

    protected void CreateAbstraction(Func<MonoAbstraction> factory)
    {
        MonoAbstraction abstraction = factory.Invoke();
        abstraction.transform.SetParent(transform,false);
        abstraction.transform.SetSiblingIndex(transform.childCount - 2);
        subAbstractions.Add(GeneralDatabase.Name("name"));
    }
}
