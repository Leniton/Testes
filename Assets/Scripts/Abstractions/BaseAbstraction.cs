using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BaseAbstraction : ICodeAbstraction
{
    public string name { get; set; }
    public List<ICodeAbstraction> subAbstractions { get; set; }

    public object Data;

    public BaseAbstraction(string _name, object data)
    {
        name = _name;
        Data = data;
    }

    public string GetCode(StringBuilder sb = null)
    {
        sb = sb ?? new StringBuilder();
        sb.Append(name);
        sb.Append("." + Data);

        return sb.ToString();
    }
}