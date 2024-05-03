using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSaturationFilter : IColorFilter
{
    private float saturation;

    public ColorSaturationFilter(float saturation)
    {
        this.saturation = Mathf.Clamp01(saturation);
        UnityEditor.EditorGUILayout.ColorField(Color.white);
    }

    public Color ApplyFilter(Color color)
    {
        float total = (color.r + color.g + color.b)/3;

        color.r = Mathf.Lerp(color.r, total, saturation);
        color.g = Mathf.Lerp(color.g, total, saturation);
        color.b = Mathf.Lerp(color.b, total, saturation);

        return color;
    }
}
