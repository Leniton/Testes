using System.Collections.Generic;
using System.Text;

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

    public string GetCode()
    {
        StringBuilder sb = new StringBuilder(name);

        sb.Append("." + Data.ToString());

        return sb.ToString();
    }
}