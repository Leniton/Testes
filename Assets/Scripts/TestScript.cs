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
        Texture2D bg = TextureHelper.GetTexPart(tex, images[11].rect);

        for (int i = 0; i < maxValue; i++)
        {
            TextureHelper.MergeTexture(finalTex, bg, new(0, bg.height * i));
        }

        RenderTexture(finalTex);
    }
}

[System.Serializable]
public struct TexData
{
    [SerializeField] int numberSpacing;
}