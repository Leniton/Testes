
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

    public static SideData Blank => new(0, nameof(Blank));
    public static int Blank_Unset => 1;
    public static int Blank_Petrified => 2;
    public static int Blank_Used => 3;
    public static int Blank_Item => 4;
    public static int Blank_Curse => 5;
    public static int Blank_Stasis => 6;
    public static int Blank_Sticky => 7;
    public static int Blank_Exert => 8;
    public static int Blank_Fumble => 9;
    public static int Add_Cleanse_SelfCleanse => 10;
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