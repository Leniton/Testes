using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newTable", menuName = "Table")]
public class Table : ScriptableObject
{
    public StringCode code = new("{", "}");
    public List<Keyword> elements = new();
}

[Serializable]
public struct StringCode
{
    public string StartCode;
    public string EndCode;

    public StringCode(string startCode, string endCode)
    {
        StartCode = startCode;
        EndCode = endCode;
    }
}