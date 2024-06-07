using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class GeneralDatabase
{
    public static Dictionary<string, Func<MonoAbstraction>> HeroPickOptions => heroPickOptions;

    public static MonoAbstraction Name(string name, Transform parent = null)
    {
        TextAbstraction text = Object.Instantiate(Resources.Load<TextAbstraction>("Prefabs/AbstractionTextWindow"));
        text.gameObject.name = $"Name: {name}";
        text.Config("n", name);

        return text;
    }

    public static MonoAbstraction HP(int value, Transform parent = null)
    {
        TextAbstraction text = Object.Instantiate(Resources.Load<TextAbstraction>("Prefabs/AbstractionTextWindow"));
        text.gameObject.name = "HP";
        text.Config("hp", value);

        return text;
    }

    private static Dictionary<string, Func<MonoAbstraction>> heroPickOptions = new Dictionary<string, Func<MonoAbstraction>>
    {
        {"Name", () => Name("") },
        {"HP", () => HP(1) }
    };
}
