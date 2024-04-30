using UnityEngine;

public class OverrideColor : IColorMerger
{
    bool ignoreAlpha;
    public OverrideColor(bool ignoreAlpha = true)
    {
        this.ignoreAlpha = ignoreAlpha;
    }

    public Color MergeColors(Color color1, Color color2) => !ignoreAlpha || color2.a >= 0.001f ? color2 : color1;
}
