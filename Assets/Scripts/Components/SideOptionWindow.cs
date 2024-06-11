using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideOptionWindow : MonoBehaviour
{
    [SerializeField] private SideOption baseOption;
    [SerializeField] private RectTransform content;
    
    private void Awake()
    {
        for (int i = 0; i < DiceSideDatabase.sides.Length; i++)
        {
            SideData data = DiceSideDatabase.sidesData[i];

            SideOption newOption = Instantiate(baseOption, content);
            newOption.gameObject.SetActive(true);
            newOption.SetUp(data);
        }
    }
}
