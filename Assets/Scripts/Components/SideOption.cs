using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SideOption : MonoBehaviour
{
    [SerializeField] private Image side;
    [SerializeField] private Image pip;
    private SideData data;

    public void SetUp(SideData newData, int pips = 0)
    {
        data = newData;

        side.sprite = data.sprite;
        Debug.Log($"{data.Id} pips: {data.pips}");
        pip.sprite = DiceSideDatabase.pips[data.pips >= 0 ? pips : DiceSideDatabase.pips.Length - 1];
    }
}
