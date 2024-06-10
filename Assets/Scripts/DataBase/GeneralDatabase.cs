using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public static class GeneralDatabase
{
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
        text.Config("hp", value, TMP_InputField.ContentType.IntegerNumber);

        return text;
    }

    public static MonoAbstraction Keyword()
    {
        SugestTextAbstraction enumerator = Object.Instantiate(Resources.Load<SugestTextAbstraction>("Prefabs/AbstractionSugestTextWindow"));
        enumerator.gameObject.name = "Keyword";
        enumerator.Config("k", KeywordDatabase.keywords);

        return enumerator;
    }
    
    public static MonoAbstraction Generated(string name)
    {
        TextAbstraction enumerator = Object.Instantiate(Resources.Load<TextAbstraction>("Prefabs/HexAbstractionWindow"));
        enumerator.gameObject.name = "Generated";
        enumerator.Config(name, "", TMP_InputField.ContentType.Custom);

        return enumerator;
    }

    public static readonly Dictionary<string, Func<MonoAbstraction>> HeroPickOptions = new()
    {
        {"Name", () => Name("") },
        {"HP", () => HP(1) }
    };
    
    public static readonly Dictionary<string, Func<MonoAbstraction>> ItemPickOptions = new()
    {
        {"Name", () => Name("") }
    };
}
