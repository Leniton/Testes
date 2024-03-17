using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Keyword 
{
    public string str;
    public string description;
    public Color color;

    public Keyword(string _str, string _description, Color? _color = null)
    {
        str = _str;
        description = _description;
        color = _color ?? Color.white;
    }

    public static implicit operator string(Keyword keyword) => keyword.str;
}
