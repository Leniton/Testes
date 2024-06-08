using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lenix.NumberUtilities;

public class DiceSideAbstraction : MonoBehaviour
{
    [SerializeField] private StateButton choseSides;
    [SerializeField] private StateButton left, midle, top, bottom, right, rightmost;
    [Space]
    [Header("Combination buttons"),SerializeField] private StateButton right2;
    [SerializeField] private StateButton topbot, row, col, all;

    private void UpdateButtons()
    {

    }
}

public enum DiceSides
{
    left = 1,
    middle = 2,
    top = 4,
    bottom = 8,
    right = 16,
    rightmost = 32,
    right2 = 0b110000,
    row = 0b110011,
    topbot = 0b001100,
    col = 0b001110,
    all = 0b111111
}