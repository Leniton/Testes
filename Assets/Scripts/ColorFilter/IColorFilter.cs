using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColorFilter
{
    public Color ApplyFilter(Color color);
}

public enum ColorChannel
{
    r, g, b, a
}