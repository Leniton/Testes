using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class GeneralDatabase
{
    public static TextAbstraction Name(string name) 
    {
        TextAbstraction text = Object.Instantiate(Resources.Load<TextAbstraction>("Prefabs/AbstractionTextWindow"));
        text.gameObject.name = $"Name: {name}";
        text.Config("n", name);

        return text;
    }
    public static ICodeAbstraction HP(int value) => new BaseAbstraction("hp", Math.Clamp(value, 0, 99));
}
