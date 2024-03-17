using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class KeywordDictionary : MonoBehaviour
{
    [SerializeField] protected List<Keyword> dictionary = new();

    public static KeywordDictionary instance;

    public static Keyword Get(int index)
    {
        if (index < 0 || index >= instance.dictionary.Count) return null;
        return instance.dictionary[index];
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
