using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class EnumAbstraction : MonoAbstraction
{
    [SerializeField] string codeName;
    [SerializeField] TMP_Dropdown dropdown;

    private BaseAbstraction baseAbstraction;

    private Type enumType;

    private void Awake()
    {
        if (!dropdown) dropdown = GetComponentInChildren<TMP_Dropdown>();

        dropdown.onValueChanged.AddListener(UpdateData);
    }

    private void UpdateData(int data)
    {
        baseAbstraction.Data = Enum.Parse(enumType, dropdown.options[data].text);
    }

    public void Config(string Name, Enum data)
    {
        enumType = data.GetType();
        baseAbstraction = new BaseAbstraction(Name, data);
        codeName = name;

        dropdown.ClearOptions();
        string[] names = Enum.GetNames(enumType);
        List<TMP_Dropdown.OptionData> options = new();
        int picked = 0;
        for (int i = 0; i < names.Length; i++)
        {
            options.Add(new(names[i]));
            if (names[i] == data.ToString()) picked = i;
        }
        dropdown.AddOptions(options);
        dropdown.value = picked;
    }

    public override string GetCode(StringBuilder sb) => baseAbstraction.GetCode(sb);
}
