using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeywordDictionary 
{
    private Dictionary<string, Keyword> dictionary = new();

    protected bool Has(string key)
    {
        return dictionary.ContainsKey(key);
    }
    protected Keyword Get(Keyword keyword)
    {
        if(Has(keyword)) dictionary[keyword] = keyword;
        return dictionary[keyword];
    }
}
