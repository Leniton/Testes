
using UnityEngine;

public static class TextureHelper
{
    public static Texture2D GetTexPart(Texture2D texture,Rect area)
    {
        Color[] texColors = texture.GetPixels();
        
        Texture2D finalTex = new Texture2D((int)area.width, (int)area.height);
        finalTex.filterMode = texture.filterMode;
        Color[] finalColors = new Color[finalTex.width * finalTex.height];
        
        for (int i = 0; i < finalColors.Length; i++)
        {
            int x = (int)area.x + (i % (int)area.width);
            int y = (int)area.y + (i / (int)area.width);
            int id = CoordinateToArray(new(x, y), texture);
            finalColors[i] = texColors[id];
        }
        finalTex.SetPixels(finalColors);
        finalTex.Apply();
        return finalTex;
    }

    public static int CoordinateToArray(Coordinate coordinate,Texture2D tex)
    {
        int id = coordinate.x;
        id += coordinate.y * tex.width;
        return id;
    }

    public static Coordinate ArrayToCoordinate(int id, int width)
    {
        return new(0, 0);
    }
}

public struct Coordinate
{
    public int x, y;

    public Coordinate(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}