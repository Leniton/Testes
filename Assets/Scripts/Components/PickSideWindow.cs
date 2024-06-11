using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSideWindow : MonoBehaviour
{
    [SerializeField] private SideOption side;
    public SideData sideData;

    private void Awake()
    {
        sideData = DiceSideDatabase.sidesData[0];
        side.SetUp(sideData);
        side.OnSelect += PickOption;
    }

    private void PickOption(SideData data)
    {
        SideOptionWindow.PickSide(data,OnPick);
    }

    private void OnPick(SideData data)
    {
        sideData = data;
        side.SetUp(data, data.pips);
    }
}
