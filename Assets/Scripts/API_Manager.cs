using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API_Manager : MonoBehaviour
{
    [SerializeField] private RectTransform MainParent;
    
    private void Awake()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/IDs Sides - Helpsheet");
        Debug.Log(Resources.Load<TextAsset>("CSV/IDs Sides - Helpsheet").GetType());
        MonoAbstraction item = WindowGenerator.Item_Generated();
        item.transform.SetParent(MainParent, false);
    }
}
