
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
                _sides = Resources.LoadAll<Sprite>("Sprites/DiceSides");
            }

            return _sides;
        }
    }

    private static Sprite[] _pips;
    public static Sprite[] pips
    {
        get
        {
            if(_pips == null)
            {
                _pips = Resources.LoadAll<Sprite>("Sprites/Pips");
            }

            return _pips;
        }
    }

    public static List<SideData> sidesData = new();
}

public struct SideData
{
    public int Id;
    public string Name;
    public Sprite sprite;
    public int pips;

    public SideData(int id, string name,int _pips)
    {
        id = id < 0 || id > DiceSideDatabase.sides.Length - 2 ? DiceSideDatabase.sides.Length - 1 : id;

        Id = id;
        Name = name;
        sprite = DiceSideDatabase.sides[id];
        pips = _pips;
    }
}