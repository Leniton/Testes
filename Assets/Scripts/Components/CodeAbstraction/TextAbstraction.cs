using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text;

public class TextAbstraction : MonoAbstraction
{
    [SerializeField] string codeName;
    [SerializeField] TMP_InputField InputField;

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
        if (!InputField) InputField = GetComponentInChildren<TMP_InputField>();

        InputField.onValueChanged.AddListener(UpdateData);
    }

    private void UpdateData(string data) => baseAbstraction.Data = data;

    public void Config(string Name, object data, TMP_InputField.ContentType type = TMP_InputField.ContentType.Standard, int characterLimit = 99999)
    {
        baseAbstraction = new BaseAbstraction(Name, data);
        codeName = name;
        InputField.contentType = type;
        InputField.characterLimit = characterLimit;
    }

    [SerializableMethods.SerializeMethod]
    public override string GetCode(StringBuilder sb) => baseAbstraction.GetCode(sb);
}
