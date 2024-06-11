using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorState : DualState
{
    [SerializeField] Color onColor, offColor;
    [SerializeField] Graphic graphic;

    public override void SetState(bool state)
    {
        graphic.color = state ? onColor : offColor;
    }
}
