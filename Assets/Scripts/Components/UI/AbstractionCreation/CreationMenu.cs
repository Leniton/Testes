using System;
using System.Collections.Generic;
using UnityEngine;

public class CreationMenu : MonoBehaviour
{
    [SerializeField] GameObject PickScreen;
    [SerializeField] OptionPicker picker;

    public Action<MonoAbstraction> onCreated;

    private Dictionary<string, Func<MonoAbstraction>> creations = new()
    {
        { "Hero - Reference", WindowGenerator.Hero_Reference },
        { "Item - Reference", WindowGenerator.Item_Reference },
        { "Item - Keyword", WindowGenerator.Item_Keyword },
        { "Item - Generated", WindowGenerator.Item_Generated },
        { "Item - GeneratedX", WindowGenerator.Item_GeneratedX },
    };

    private void Awake()
    {
        picker.onPick += Pick;
        SetOptions();
    }

    private void SetOptions()
    {
        List<string> dropdownOptions = new();
        foreach (string option in creations.Keys)
        {
            dropdownOptions.Add(option);
        }

        picker.SetOptions(dropdownOptions);
    }

    public void OpenOptions()
    {
        PickScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        PickScreen.SetActive(false);
    }

    private void Pick(int id)
    {
        string key = picker.options[id].Text;

        onCreated?.Invoke(creations[key].Invoke());
        CloseOptions();
    }
}