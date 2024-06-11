using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickSideWindow : MonoBehaviour
{
    [SerializeField] private SideOption side;

    private void Awake()
    {
        side.SetUp(DiceSideDatabase.sidesData[0]);
        side.OnSelect += PickOption;
    }

    private void PickOption(SideData data)
    {
        SideOptionWindow.PickSide(data,OnPick);
    }

    private void OnPick(SideData data)
    {
        side.SetUp(data, data.pips);
    }
}
