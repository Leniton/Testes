
using System.Collections.Generic;
using UnityEngine;

public static class DiceSideDatabase
{
    private static Sprite[] _sides;
    public static Sprite[] sides
    {
        get
        {
            if(_sides == null)
            {
                _sides = Resources.LoadAll<Sprite>("Sprites");
            }

            return _sides;
        }
    }

    public static List<SideData> sidesData = new();
}

public struct SideData
{
    public int Id;
    public string Name;
    public Sprite sprite;

    public SideData(int id, string name)
    {
        id = id < 0 || id > DiceSideDatabase.sides.Length - 2 ? DiceSideDatabase.sides.Length - 1 : id;

        Id = id;
        Name = name;
        sprite = DiceSideDatabase.sides[id];
    }
}