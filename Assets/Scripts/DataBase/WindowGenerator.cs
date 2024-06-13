using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class WindowGenerator
{
    public static MonoAbstraction Name()
    {
        TextAbstraction text = Object.Instantiate(Resources.Load<TextAbstraction>("Prefabs/AbstractionTextWindow"));
        text.gameObject.name = $"Name";
        text.Config("n", "");
        text.Style("Name");

        return text;
    }
    public static MonoAbstraction HP()
    {
        TextAbstraction text = Object.Instantiate(Resources.Load<TextAbstraction>("Prefabs/AbstractionTextWindow"));
        text.gameObject.name = "HP";
        text.Config("hp", 0, TMP_InputField.ContentType.IntegerNumber);
        text.Style("HP", Color.red);

        return text;
    }

    public static MonoAbstraction Reference(string Title, List<string> referencePool, List<string> referenceDescription = null, string codeName = "")
    {
        SugestTextAbstraction reference = Object.Instantiate(Resources.Load<SugestTextAbstraction>("Prefabs/AbstractionSugestTextWindow"));
        reference.gameObject.name = $"Reference: {Title}";
        reference.Config(codeName, referencePool, referenceDescription);
        reference.Style(Title);

        return reference;
    }
    public static MonoAbstraction Keyword() => Reference("Keyword", KeywordDatabase.keywords, KeywordDatabase.desctiptions, "k");
    public static MonoAbstraction Generated(string name)
    {
        TextAbstraction generated = Object.Instantiate(Resources.Load<TextAbstraction>("Prefabs/HexAbstractionWindow"));
        generated.gameObject.name = "Generated";
        generated.Config(name, "", TMP_InputField.ContentType.Custom);

        return generated;
    }

    public static MonoAbstraction Item_Reference()
    {
        ItemAbstraction item = Object.Instantiate(Resources.Load<ItemAbstraction>("Prefabs/ItemAbstractionWindow"));
        item.gameObject.name = "Item";
        item.SetItem(Reference("Item",ItemDatabase.Items,ItemDatabase.desctiptions));

        return item;
    }
    public static MonoAbstraction Item_Keyword()
    {
        ItemAbstraction item = Object.Instantiate(Resources.Load<ItemAbstraction>("Prefabs/ItemAbstractionWindow"));
        item.gameObject.name = "Item";
        item.SetItem(Keyword());

        return item;
    }
    public static MonoAbstraction Item_Generated()
    {
        ItemAbstraction item = Object.Instantiate(Resources.Load<ItemAbstraction>("Prefabs/ItemAbstractionWindow"));
        item.gameObject.name = "Item";
        item.SetItem(Generated("ritem"));

        return item;
    }
    public static MonoAbstraction Item_GeneratedX()
    {
        ItemAbstraction item = Object.Instantiate(Resources.Load<ItemAbstraction>("Prefabs/ItemAbstractionWindow"));
        item.gameObject.name = "Item";
        item.SetItem(Generated("ritemx"));

        return item;
    }

    public static MonoAbstraction Hero_Reference()
    {
        HeroAbstraction hero = Object.Instantiate(Resources.Load<HeroAbstraction>("Prefabs/HeroAbstractionWindow"));
        hero.gameObject.name = "Hero";
        hero.SetHero(Reference("Hero", HeroDatabase.Heroes, HeroDatabase.desctiptions));

        return hero;
    }
    public static MonoAbstraction Hero_Generated(HeroColor color, int tier)
    {
        HeroAbstraction hero = Object.Instantiate(Resources.Load<HeroAbstraction>("Prefabs/HeroAbstractionWindow"));
        hero.gameObject.name = "Hero";
        hero.SetHero(Generated($"{color}{tier}"));

        return hero;
    }
}
