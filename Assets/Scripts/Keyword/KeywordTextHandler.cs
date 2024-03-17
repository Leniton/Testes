using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeywordTextHandler : MonoBehaviour
{
    [SerializeField] TMP_Text uiText;
    public TMP_Text Text => uiText;
    public string text
    {
        get => uiText.text;
        set => uiText.text = ApplyText(value);
    }
    private void Awake()
    {
        if(uiText == null) uiText = GetComponent<TMP_Text>();
        uiText.richText = true;
        text = uiText.text;
    }

    private string ApplyText(string value, int startIndex = 0)
    {
        int index = value.IndexOf('{', startIndex);
        int id;
        if (!ValidKey(value, index, out id)) return value;
        //print($"id is {id}");
        Keyword keyword = KeywordDictionary.Get(id);
        string newValue = keyword == null ? value : value.Replace($"{{{id}}}", $"<link=\"{id}\">{keyword}</link>");
        return ApplyText(newValue, index + 1);
    }

    private bool ValidKey(string value, int startIndex, out int index)
    {
        index = -1;
        if(startIndex < 0 || value.Length - startIndex < 3) return false;
        int id = value.IndexOf('}', startIndex);
        if (id < 0) return false;
        string keyString = value.Substring(startIndex + 1, id - startIndex - 1);
        //print($"keystring is {keyString}");
        if (!int.TryParse(keyString, out index)) index = -1;
        return true;
    }
}
