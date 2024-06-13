using System;
using System.Collections.Generic;

public static class GeneralDatabase
{
    public static readonly Dictionary<string, Func<MonoAbstraction>> HeroPickOptions = new()
    {
        { "Name", ()=> WindowGenerator.Word("name","n") },
        { "HP", ()=> WindowGenerator.Number("HP","hp") },
        { "Item - Reference", WindowGenerator.Item_Reference },
        { "Item - Keyword", WindowGenerator.Item_Keyword },
        { "Item - Generated", WindowGenerator.Item_Generated },
        { "Item - GeneratedX", WindowGenerator.Item_GeneratedX },
    };

    public static readonly Dictionary<string, Func<MonoAbstraction>> ItemPickOptions = new()
    {
        {"Name", ()=> WindowGenerator.Word("name","n") }
    };
}
