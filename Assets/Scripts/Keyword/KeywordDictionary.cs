using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class KeywordDictionary : MonoBehaviour
{
    [SerializeField] protected List<Keyword> dictionary = new();

    public static KeywordDictionary instance;

    public Keyword Get(int index)
    {
        if (index < 0 || index >= dictionary.Count) return null;
        return dictionary[index];
    }

    private void Awake()
    {
        if (instance != this)
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
            }
            else Destroy(gameObject);
        }
    }
}
