
using Unity.VisualScripting;
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

    public static void MergeTexture(Texture2D baseTexture, Texture2D insertedTexture, Vector2Int offset, IColorMerger colorMerger = null)
    {
        colorMerger = colorMerger ?? new OverrideColor();
        Color[] baseColor = baseTexture.GetPixels();
        Color[] insertColor = insertedTexture.GetPixels();

        for (int x = 0; x < insertedTexture.width; x++)
        {
            for (int y = 0; y < insertedTexture.height; y++)
            {
                Vector2Int coordinate = new Vector2Int(x, y);
                Color color = insertColor[CoordinateToArray(coordinate, insertedTexture)];
                coordinate.x += offset.x;
                coordinate.y += offset.y;
                int id = CoordinateToArray(coordinate, baseTexture);
                if (id >= 0 && id < baseColor.Length) baseColor[id] = colorMerger.MergeColors(baseColor[id], color);
            }
        }

        baseTexture.SetPixels(baseColor);
        baseTexture.Apply();
    }

    public static int CoordinateToArray(Vector2Int coordinate,Texture2D tex)
    {
        int id = coordinate.x;
        id += coordinate.y * tex.width;
        return id;
    }
    public static int CoordinateToArray(int x, int y, Texture2D tex)
    {
        int id = x;
        id += y * tex.width;
        return id;
    }

    public static Vector2Int ArrayToCoordinate(int id, int width)
    {
        Vector2Int coordinate = new Vector2Int();
        coordinate.x = id % width;
        coordinate.y = id / width;

        return coordinate;
    }
}