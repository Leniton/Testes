using System;
using System.Collections.Generic;

public static class GeneralDatabase
{
    public static readonly Dictionary<string, Func<MonoAbstraction>> HeroPickOptions = new()
    {
        { "Name", ()=> WindowGenerator.Word("name","n") },
        { "Tier", ()=> WindowGenerator.Number("tier","tier") },
        { "HP", ()=> WindowGenerator.Number("HP","hp") },
        { "Adj", ()=> WindowGenerator.Number("Adj","adj") },
        { "Item - Reference", WindowGenerator.Item_Reference },
        { "Item - Keyword", WindowGenerator.Item_Keyword },
        { "Item - Generated", WindowGenerator.Item_Generated },
        { "Item - GeneratedX", WindowGenerator.Item_GeneratedX },
    };

    public static readonly Dictionary<string, Func<MonoAbstraction>> ItemPickOptions = new()
    {
        {"Name", ()=> WindowGenerator.Word("name","n") },
        { "Multiply", ()=> WindowGenerator.Number("Mult","m") },
        { "Tier", ()=> WindowGenerator.Number("tier","tier") },
    };
}
