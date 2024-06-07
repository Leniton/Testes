using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionItem : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text;

    public string Text => text.text;

    public Action OnClick;

    private void Awake()
    {
        if (!button) button = GetComponentInChildren<Button>();

        button.onClick.AddListener(Clicked);
    }

    private void Clicked() => OnClick?.Invoke();

    public void SetUp(string txt)
    {
        if (!text) text = GetComponentInChildren<TMP_Text>();

        text.text = txt;
    }
}
