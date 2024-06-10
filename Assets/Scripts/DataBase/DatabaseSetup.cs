using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseSetup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void SetUp()
    {
        DiceSideSetup();
    }

    public static void DiceSideSetup()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/IDs Sides - Helpsheet");
        string[,] idSideData = CSVLoader.LoadCSV(CSVLoader.GetReaderFromString(textAsset.text));
        int startingIndex = 2;
        int length = idSideData.GetLength(0);

        //1: id | 2: name | 3: usePips(FALSE,TRUE)
        for (int i = startingIndex; i < length; i++)
        {
            int.TryParse(idSideData[i, 1], out int id);
            string name = idSideData[i, 2];
            DiceSideDatabase.sidesData.Add(new SideData(id, name));
        }
    }
}
