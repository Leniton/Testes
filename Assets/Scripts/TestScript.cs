using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    [SerializeField] private Texture2D tex;
    private RawImage image;
    [SerializeField] private Vector2Int pos, size;
    void Awake()
    {
        image = GetComponent<RawImage>();
        //RenderTexture();
        StartCoroutine(UpdateTexture());
    }

    private void RenderTexture()
    {
        pos.x = Mathf.Clamp(pos.x, 0, tex.width);
        pos.y = Mathf.Clamp(pos.y, 0, tex.height);
        size.x = Mathf.Clamp(size.x, 0, tex.width - pos.x);
        size.y = Mathf.Clamp(size.y, 0, tex.height - pos.y);
        Rect finalRect = new(pos.x,pos.y,size.x,size.y);
        
        image.texture = TextureHelper.GetTexPart(tex, finalRect);
        image.rectTransform.sizeDelta = new(image.texture.width, image.texture.height);
    }

    IEnumerator UpdateTexture()
    {
        while (true)
        {
            RenderTexture();
            yield return new WaitForSeconds(.6f);
        }
    }
}
