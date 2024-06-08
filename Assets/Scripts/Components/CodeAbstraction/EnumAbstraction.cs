using System;
using System.Text;
using UnityEngine;

public class EnumAbstraction : MonoAbstraction
{
    [SerializeField] string codeName;
    [SerializeField] private OptionPicker optionPicker;
    
    private BaseAbstraction baseAbstraction;

    private Enum value;

    private void Awake()
    {
        if (!optionPicker) optionPicker = GetComponentInChildren<OptionPicker>();
        
        optionPicker.onPick+= UpdateData;
    }

    private void UpdateData(int data)
    {
        baseAbstraction.Data = data;
    }

    public void Config(string Name, Enum data)
    {
        baseAbstraction = new BaseAbstraction(Name, data);
        codeName = name;

        optionPicker.SetOptions(new(Enum.GetNames(data.GetType())));
    }

    public override string GetCode(StringBuilder sb = null) => baseAbstraction.GetCode(sb);
}
