using System;
using TMPro;
using UnityEngine;

public class ValueInputField : MonoBehaviour
{
    [SerializeField] private int minValue,maxValue;
    private TMP_InputField input;

    public Action<int> onValueChanged;

    public int Value
    {
        get => int.Parse(input.text);
        set => input.text = (Mathf.Clamp(value, minValue, maxValue)).ToString();
    }

    private void Awake()
    {
        input = GetComponent<TMP_InputField>();
        input.contentType = TMP_InputField.ContentType.IntegerNumber;
        
        input.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string txt)
    {
        onValueChanged?.Invoke(Value);
    }
}
