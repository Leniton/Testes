using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FilterTest : MonoBehaviour
{
    public ColorChannel channel;
    [SerializeField] RawImage image;
    private Texture2D tex;

    int blockSize = 80;

    void Awake()
    {
        CreateTestTex();
        //StartCoroutine(Test());
    }

    private void CreateTestTex()
    {
        tex = TextureHelper.FlatTexture(blockSize * 3, blockSize * 2);
        tex.filterMode = FilterMode.Point;

        //Red
        Color color = Color.red;
        Vector2Int offset = Vector2Int.zero;
        AddColor(color, offset);
        color.g = 1;
        offset.x = 80;
        AddColor(color, offset);
        color.g = 0;
        color.b = 1;
        offset.x = 80 * 2;
        AddColor(color, offset);

        //Green
        color = Color.green;
        offset = Vector2Int.up * 80;
        AddColor(color, offset);
        color.b = 1;
        offset.x = 80;
        AddColor(color, offset);
        color = Color.blue;
        offset.x = 80 * 2;
        AddColor(color, offset);

        image.texture = tex;
    }

    private void AddColor(Color color, Vector2Int offset)
    {
        Texture2D subTex = TextureHelper.FlatTexture(blockSize, blockSize, color);
        TextureHelper.MergeTexture(tex, subTex, offset);
    }

    private void UseFilter()
    {
        IColorFilter filter = new ColorChannelFilter(channel);

        TextureHelper.ApplyFilter(tex, filter);
    }

    IEnumerator Test()
    {
        while (true)
        {
            UseFilter();
            yield return new WaitForSeconds(.4f);
        }
    }
}
