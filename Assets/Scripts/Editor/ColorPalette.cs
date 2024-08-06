using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ColorPalette", fileName = "newColorPalette")]
public class ColorPalette : ScriptableObject
{
    public List<Graphic> Graphics = new();
    public List<Renderer> Renderers = new();

    public List<int> _graphicsLookUp = new();
    public List<int> _renderersLookUp = new();
    
    public List<Color> Colors = new();
}
