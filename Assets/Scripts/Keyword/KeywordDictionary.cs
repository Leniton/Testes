using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class KeywordDictionary : MonoBehaviour
{
    protected const string divisor = "table";
    protected List<Table> tables = new();

    public static KeywordDictionary instance;

    public static List<Table> Tables => instance.tables;

    public static Keyword Get(string key)
    {
        List<Table> tables = instance.tables;
        if (!key.Contains(divisor)) return null;

        string tableIndex = key.Substring(0, key.IndexOf(divisor));
        if (int.TryParse(tableIndex, out int tableId) && ContainsID(tables, tableId))
        {
            string index = key.Remove(0, tableIndex.Length + divisor.Length);
            if (int.TryParse(index, out int id) && 
                ContainsID(tables[tableId].elements, id))
                return tables[tableId].elements[id];
        }

        return null;
    }

    private void Awake()
    {
        if (instance != this)
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
                Init();
            }
            else Destroy(gameObject);
        }
    }

    private void Init()
    {
        Table[] list = Resources.LoadAll<Table>("ScriptableObjects");
        for (int i = 0; i < list.Length; i++)
        {
            tables.Add(list[i]);
        }
    }

    public static string TableIdToCode(Table table, int id)
    {
        return $"{Tables.IndexOf(table)}{divisor}{id}";
    }

    private static bool ContainsID(ICollection collection, int id) => id >= 0 && id < collection.Count;
}
