using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAbstraction : MonoBehaviour, ICodeAbstraction
{
    [SerializeField] string codeName;
    [SerializeField] TMP_InputField InputField;

    public string name { get => baseAbstraction.name; set => baseAbstraction.name = value; }
    public List<ICodeAbstraction> subAbstractions { get; set; }

    private BaseAbstraction baseAbstraction;

    private void Awake()
    {
        if(!InputField) InputField = GetComponentInChildren<TMP_InputField>();

        InputField.onValueChanged.AddListener(UpdateData);

        Config(codeName, InputField.text);
    }

    private void UpdateData(string data) => baseAbstraction.Data = data;

    public TextAbstraction Config(string Name, object data)
    {
        baseAbstraction = new BaseAbstraction(Name, data);
        codeName = name;
        return this;
    }

    [SerializableMethods.SerializeMethod]
    public string GetCode() => baseAbstraction.GetCode();
}
