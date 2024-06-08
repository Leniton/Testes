using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text;

public class TextAbstraction : MonoAbstraction
{
    [SerializeField] string codeName;
    [SerializeField] TMP_InputField InputField;

    public string name
    {
        get => baseAbstraction.name;
        set => baseAbstraction.name = value;
    }

    public List<ICodeAbstraction> subAbstractions
    {
        get => baseAbstraction.subAbstractions;
        set => baseAbstraction.subAbstractions = value;
    }

    private BaseAbstraction baseAbstraction;

    private void Awake()
    {
        if (!InputField) InputField = GetComponentInChildren<TMP_InputField>();

        InputField.onValueChanged.AddListener(UpdateData);

        Config(codeName, InputField.text);
    }

    private void UpdateData(string data) => baseAbstraction.Data = data;

    public void Config(string Name, object data, TMP_InputField.ContentType type = TMP_InputField.ContentType.Standard)
    {
        baseAbstraction = new BaseAbstraction(Name, data);
        codeName = name;
        InputField.contentType = type;
    }

    [SerializableMethods.SerializeMethod]
    public override string GetCode(StringBuilder sb = null) => baseAbstraction.GetCode(sb);
}
