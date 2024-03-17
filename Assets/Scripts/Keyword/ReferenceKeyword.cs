using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceKeyword<T> : Keyword
{
    public T reference;

    public ReferenceKeyword(string _str, string _description,T _reference, Color? _color = null) : base(_str, _description, _color)
    {
        reference = _reference;
    }

    public static implicit operator T(ReferenceKeyword<T> keyword) => keyword.reference;
}
