using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    [SerializeField] private Texture2D tex;
    private RawImage image;
    [SerializeField] private int index, min;
    [SerializeField] private Sprite[] images;
    [SerializeField] int numberSpacing;
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
        Texture2D finalTex = new Texture2D((int)images[11].rect.width, (maxValue-min) * (int)images[11].rect.height);
        finalTex.filterMode = FilterMode.Point;
        Texture2D bg = TextureHelper.GetTexPart(tex, images[11].rect);

        for (int i = min; i < maxValue; i++)
        {
            TextureHelper.MergeTexture(finalTex, bg, new(0, bg.height * (i-min)));
        }

        //for now clamped at 9
        int values = maxValue;//Mathf.Min(maxValue, 9);
        int width = (int)images[0].rect.width;
        int height = (int)images[0].rect.height;
        for (int i = min; i < values; i++)
        {
            Vector2Int offset = new Vector2Int(0, bg.height * (i-min));
            offset.x = (bg.width / 2);// - (number.width/2);
            offset.y += (bg.height / 2) - (height/2);

            
            //offset for multiple numbers
            int numbers = 1;
            int ten = 10;
            //verify up to 100000
            for (int o = 0; o < 5; o++)
            {
                if ((i+1) / ten > 0)
                {
                    numbers++;
                }
                ten *= 10;
            }
            //add spacing
            int fullSpace = width * numbers + (numberSpacing * (numbers - 1));
            int initialOffset = fullSpace / 2;
            int spacing = width + numberSpacing;
            offset.x -= initialOffset;
            for (int s = numbers-1; s >= 0; s--)
            {
                Texture2D number = TextureHelper.GetTexPart(tex, images[GetDigit(i + 1, s)].rect);
                TextureHelper.MergeTexture(finalTex, number, offset);
                offset.x += spacing;
            }

        }

        RenderTexture(finalTex);
    }

    private int GetDigit(int number, int digitID = 0)
    {
        int value = number;
        if (digitID > 0) value = Mathf.FloorToInt(number / Mathf.Pow(10, digitID));
        return value % 10;
    }
}

[System.Serializable]
public struct TexData
{
    [SerializeField] int numberSpacing;
}