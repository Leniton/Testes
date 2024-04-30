using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    [SerializeField] private Texture2D tex;
    private RawImage image;
    [SerializeField,Range(0,11)] private int index;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TexData texData;
    private bool isNumber => index < 10;

    void Awake()
    {
        image = GetComponent<RawImage>();
        //RenderTexture();
        //StartCoroutine(UpdateTexture());
        //MountTexture();
        StartCoroutine(TestNewTex());
    }

    private void RenderTexture(Texture2D texture = null)
    {
        image.texture = texture ?? tex;
        image.rectTransform.sizeDelta = new(image.texture.width, image.texture.height);
    }

    IEnumerator UpdateTexture()
    {
        while (true)
        {
            Rect finalRect = images[index].rect;
            RenderTexture(TextureHelper.GetTexPart(tex, finalRect));
            yield return new WaitForSeconds(.4f);
        }
    }

    IEnumerator TestNewTex()
    {
        while (true)
        {
            MountTexture(index);
            yield return new WaitForSeconds(.4f);
        }
    }

    private void MountTexture(int maxValue = 10)
    {
        Texture2D finalTex = new Texture2D((int)images[11].rect.width, maxValue * (int)images[11].rect.height);
        finalTex.filterMode = FilterMode.Point;
        Texture2D bg = TextureHelper.GetTexPart(tex, images[11].rect);

        for (int i = 0; i < maxValue; i++)
        {
            TextureHelper.MergeTexture(finalTex, bg, new(0, bg.height * i));
        }

        //for now clamped at 9
        int values = Mathf.Min(maxValue, 9);
        for (int i = 0; i < values; i++)
        {
            Texture2D number = TextureHelper.GetTexPart(tex, images[i + 1].rect);
            Vector2Int offset = new Vector2Int(0, bg.height * i);// new((bg.width / 2), (bg.height / 2));
            offset.x = (bg.width / 2) - (number.width/2);
            offset.y += (bg.height / 2) - (number.height/2);

            TextureHelper.MergeTexture(finalTex, number, offset);
        }

        RenderTexture(finalTex);
    }
}

[System.Serializable]
public struct TexData
{
    [SerializeField] int numberSpacing;
}