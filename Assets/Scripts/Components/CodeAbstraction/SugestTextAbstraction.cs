using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SugestTextAbstraction : MonoAbstraction
{
    [SerializeField] string codeName;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] ScrollRect scroll;
    [SerializeField] private OptionItem optionItem;

    private List<string> lines;
    private List<OptionItem> options = new();

    public override string name
    {
        get => baseAbstraction.name;
        set => baseAbstraction.name = value;
    }

    public override List<ICodeAbstraction> subAbstractions
    {
        get => baseAbstraction.subAbstractions;
        set => baseAbstraction.subAbstractions = value;
    }

    private BaseAbstraction baseAbstraction;

    private void Awake()
    {
        optionItem.transform.SetParent(scroll.transform, false);
        optionItem.gameObject.SetActive(false);
        CloseOptions();

        inputField.onValueChanged.AddListener(CheckText);
    }

    private void CheckText(string data)
    {
        baseAbstraction.Data = data;
        if (string.IsNullOrEmpty(data))
        {
            CloseOptions();
            return;
        }

        List<string> sugestions = new();

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contains(data))
            {
                inputField.textComponent.color = lines[i] == data ? Color.green : Color.white;
                sugestions.Add(lines[i]);
            }

            if (sugestions.Count > 10)
            {
                CloseOptions();
                return;
            }
        }

        OpenOptions();
        SetOptions(sugestions);
    }
    public void Config(string Name, List<string> data)
    {
        baseAbstraction = new BaseAbstraction(Name, data);
        codeName = name;
        lines = data;
    }
    public void OpenOptions()
    {
        scroll.gameObject.SetActive(true);
    }
    public void CloseOptions()
    {
        scroll.gameObject.SetActive(false);
    }
    public void SetOptions(List<string> _options)
    {
        //refactor: only delete the not needed ones
        ClearOptions();

        for (int i = 0; i < _options.Count; i++)
        {
            int id = i;
            OptionItem item = Instantiate(optionItem, scroll.content);
            item.gameObject.SetActive(true);
            item.SetUp(_options[id]);
            item.OnClick += () => PickOption(id);
            options.Add(item);
        }
    }
    private void PickOption(int id)
    {
        inputField.text = options[id].Text;
        CloseOptions();
    }
    public void ClearOptions()
    {
        int childs = options.Count;
        for (int i = 0; i < childs; i++)
        {
            Destroy(options[i].gameObject);
        }
        options.Clear();
    }

    public override string GetCode(StringBuilder sb) => baseAbstraction.GetCode(sb);
}
