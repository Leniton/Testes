using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChannelFilter : IColorFilter
{
    private ColorChannel channel;

    public ColorChannelFilter (ColorChannel channel) => this.channel = channel;

    public Color ApplyFilter(Color color)
    {
        float finalValue = channel switch
        {
            ColorChannel.r => color.r,
            ColorChannel.g => color.g,
            ColorChannel.b => color.b
        };

        float medium = 0;
        if (color.r > 0.001f) medium++;
        if (color.g > 0.001f) medium++;
        if (color.b > 0.001f) medium++;



        color.r = finalValue;
        color.g = finalValue;
        color.b = finalValue;

        return color; 
    }
}
