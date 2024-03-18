using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class KeywordTextHandler : MonoBehaviour
{
    [SerializeField] TMP_Text uiText;
    public TMP_Text Text => uiText;
    StringBuilder sb;
    public string text
    {
        get => uiText.text;
        set
        {
            sb.Clear();
            sb.Append(value);
            List<Table> tables = KeywordDictionary.Tables;
            for (int i = 0; i < tables.Count; i++)
            {
                ApplyText(tables[i]);
            }
            uiText.text = sb.ToString();
        }
    }
    private void Awake()
    {
        if(uiText == null) uiText = GetComponent<TMP_Text>();
        uiText.richText = true;
        sb = new StringBuilder(text);
        text = uiText.text;
    }

    private void ApplyText(Table table ,int startIndex = 0)
    {
        string value = sb.ToString();
        int codeIndex = value.IndexOf(table.code.StartCode, startIndex);
        int id;
        if (!ValidKey(table.code, codeIndex, out id)) return;
        //print($"id is {id}");
        if(id > 0 || id < table.elements.Count)
        {
            Keyword keyword = table.elements[id];
            if (keyword != null)
            {
                string code = $"{table.code.StartCode}{id}{table.code.EndCode}";
                sb.Replace($"{code}", $"<link=\"{KeywordDictionary.TableIdToCode(table, id)}\">{keyword}</link>");
            }
        }
        ApplyText(table,codeIndex + 1);
    }

    private bool ValidKey(StringCode code, int startIndex, out int index)
    {
        string value = sb.ToString();
        index = -1;
        if(startIndex < 0 || value.Length - startIndex < 3) return false;
        int id = value.IndexOf(code.EndCode, startIndex);
        if (id < 0) return false;
        string keyString = value.Substring(startIndex + 1, id - startIndex - 1);
        //print($"keystring is {keyString}");
        if (!int.TryParse(keyString, out index)) index = -1;
        return true;
    }
}
