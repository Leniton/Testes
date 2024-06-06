using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ParentAbstraction : MonoAbstraction 
{
    [SerializeField] private TMP_Dropdown _dropdown;
    protected Dictionary<string, Func<MonoAbstraction>> options;
    protected TMP_Dropdown Dropdown
    {
        get
        {
            if(!_dropdown) _dropdown = GetComponentInChildren<TMP_Dropdown>();
            return _dropdown;
        }
    }

    protected void SetOptions(Dictionary<string, Func<MonoAbstraction>> dictionary)
    {
        options = dictionary;

        List<TMP_Dropdown.OptionData> dropdownOptions = new ();
        foreach (string option in options.Keys) 
        {
            dropdownOptions.Add(new(option));
        }

        Dropdown.options = dropdownOptions;
        Dropdown.onValueChanged.AddListener(PickAbstraction);
    }

    protected void PickAbstraction(int index)
    {
        string key = Dropdown.options[index].text;

        CreateAbstraction(options[key]);
    }

    protected void CreateAbstraction(Func<MonoAbstraction> factory)
    {
        MonoAbstraction abstraction = factory.Invoke();
        abstraction.transform.SetParent(transform);
        abstraction.transform.SetSiblingIndex(transform.childCount - 2);
        subAbstractions.Add(GeneralDatabase.Name("name"));
    }
}
