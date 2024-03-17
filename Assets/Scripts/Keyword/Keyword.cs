using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Keyword 
{
    public string str;
    public Color color;
    [Multiline]public string description;
    private string coloredText => $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{str}</color>";

    public Keyword(string _str, string _description, Color? _color = null)
    {
        str = _str;
        description = _description;
        color = _color ?? Color.white;
    }

    public static implicit operator string(Keyword keyword) => keyword.coloredText;
}
