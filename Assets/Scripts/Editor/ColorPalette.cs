using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ColorPalette", fileName = "newColorPalette")]
public class ColorPalette : ScriptableObject
{
    public string[] searchFolders = new string[] { "Assets" };

    public List<Color> Colors = new();

    public List<Graphic> Graphics = new();
    public List<SpriteRenderer> Renderers = new();

    public List<int> graphicsLookUp = new();
    public List<int> renderersLookUp = new();
}
