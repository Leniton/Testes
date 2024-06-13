using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseSetup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void SetUp()
    {
        DiceSideSetup();
        HeroesSetup();
        ItemsSetup();
        KeywordsSetup();
    }

    public static void DiceSideSetup()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/IDs Sides - Helpsheet");
        string[,] idSideData = CSVLoader.LoadCSV(CSVLoader.GetReaderFromString(textAsset.text));
        DiceSideDatabase.sidesData.Clear();
        int startingIndex = 2;
        int length = idSideData.GetLength(0);

        //1: id | 2: name | 3: usePips(FALSE,TRUE)
        for (int i = startingIndex; i < length; i++)
        {
            int.TryParse(idSideData[i, 1], out int id);
            string name = idSideData[i, 2];
            DiceSideDatabase.sidesData.Add(new SideData(id, name, idSideData[i, 3] == "TRUE" ? 0 : -1));
        }
    }

    public static void HeroesSetup()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/Slice & Dice Full Almanac v3.0  - Heroes");
        string[,] data = CSVLoader.LoadCSV(CSVLoader.GetReaderFromString(textAsset.text));
        int startingIndex = 3; //where data starts
        int length = data.GetLength(0);
        HeroDatabase.Heroes.Clear();

        //0: color | 1: tier | 3: name | 4: hp
        for (int i = startingIndex; i < length; i++)
        {
            string name = data[i, 3];
            HeroDatabase.Heroes.Add(name.Trim());
            HeroDatabase.desctiptions.Add($"Tier {data[i, 1]} {data[i,0]} hero: {data[i, 4]} hp");
        }
    }
    public static void ItemsSetup()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/Slice & Dice Full Almanac v3.0  - Items");
        string[,] data = CSVLoader.LoadCSV(CSVLoader.GetReaderFromString(textAsset.text));
        int startingIndex = 2; //where data starts
        int length = data.GetLength(0);
        ItemDatabase.Items.Clear();

        //0: name | 1: tier | 3: effects
        for (int i = startingIndex; i < length; i++)
        {
            string name = data[i, 0];
            ItemDatabase.Items.Add(name.Trim());
            ItemDatabase.desctiptions.Add($"[tier {data[i, 1]}] - {data[i, 3]}");
        }
    }
    public static void KeywordsSetup()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/Slice & Dice Full Almanac v3.0  - Keywords");
        string[,] data = CSVLoader.LoadCSV(CSVLoader.GetReaderFromString(textAsset.text));
        int startingIndex = 2; //where data starts
        int length = data.GetLength(0);
        KeywordDatabase.keywords.Clear();

        //0: name | 2: color | 3: effects
        for (int i = startingIndex; i < length; i++)
        {
            string name = data[i, 0];
            KeywordDatabase.keywords.Add(name.Trim());
            KeywordDatabase.desctiptions.Add(data[i, 3]);
        }
    }
}
