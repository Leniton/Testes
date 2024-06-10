using System;
using System.Collections.Generic;

public static class GeneralDatabase
{
    public static readonly Dictionary<string, Func<MonoAbstraction>> HeroPickOptions = new()
    {
        {"Name", () => WindowGenerator.Name("") },
        {"HP", () => WindowGenerator.HP(1) }
    };
    
    public static readonly Dictionary<string, Func<MonoAbstraction>> ItemPickOptions = new()
    {
        {"Name", () => WindowGenerator.Name("") }
    };
}
