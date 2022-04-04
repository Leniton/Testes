using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField]
    float minZoom = 1,maxZoom;
    [SerializeField] float currentZoom,zoomRate,moveMod;

    [Space]
    [SerializeField] RectTransform contentArea;
    [SerializeField] RectTransform map;

    Vector2 pivot;
    Vector2 offset;
    Vector3 mousePosition;

    void Start()
    {
        pivot = map.pivot;
        currentZoom = map.localScale.x;

        //offset = map.anchoredPosition - map.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            StartCoroutine(zoomMap());
        }

        //map.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = (contentArea.anchoredPosition - offset)/currentZoom;
        
        if (Input.GetMouseButtonDown(0))
        {
            currentZoom = map.localScale.x;
            //pivot = map.pivot;

            Vector2 v = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            float size = (1 / currentZoom);
            float minX = pivot.x * ((currentZoom - 1) / currentZoom);
            float minY = pivot.y * ((currentZoom - 1) / currentZoom);
            float maxX = minX + (size / 1);
            float maxY = minY + (size / 1);

            v.Set(Mathf.Lerp(minX,maxX,v.x), Mathf.Lerp(minY, maxY, v.y));
            /*print(new Vector2(minX, minY).ToString("n2"));
            print(new Vector2(maxX, maxY).ToString("n2"));
            print(v.ToString("n2"));*/

            //map.pivot = v;
            //map.anchoredPosition = new Vector2((map.rect.width * map.pivot.x) - map.rect.width / 2, (map.rect.height * map.pivot.y) - map.rect.height / 2);
        }
    }

    IEnumerator zoomMap()
    {
        if ((currentZoom + (zoomRate * Input.mouseScrollDelta.y)) < minZoom ||
            (currentZoom + (zoomRate * Input.mouseScrollDelta.y)) > maxZoom) yield break;
        
        currentZoom = Mathf.Clamp(currentZoom + (zoomRate * Input.mouseScrollDelta.y), minZoom, maxZoom);

        //pivot = map.pivot;

        Vector2 v = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 mp = v;//currentZoom == 1 ? Vector2.zero : v;

        //previous pivot (after zoom fix)
        float prevZoom = currentZoom - zoomRate;
        float asize = (1 / prevZoom);
        float aminX = pivot.x * ((prevZoom - 1) / prevZoom);
        float aminY = pivot.y * ((prevZoom - 1) / prevZoom);
        float amaxX = aminX + (asize / 1);
        float amaxY = aminY + (asize / 1);

        Vector2 a = new Vector2(Mathf.Lerp(aminX, amaxX, v.x), Mathf.Lerp(aminY, amaxY, v.y));
        /*print(new Vector2(aminX, aminY).ToString("n2"));
        print(new Vector2(amaxX, amaxY).ToString("n2"));
        print(a.ToString("n2"));
        print("----------------------");*/

        //current pivot (the one it will be used)
        float size = (1 / currentZoom);
        float minX = pivot.x * ((currentZoom - 1) / currentZoom);
        float minY = pivot.y * ((currentZoom - 1) / currentZoom);
        float maxX = minX + (size / 1);
        float maxY = minY + (size / 1);
        //print(size);

        v.Set(Mathf.Lerp(minX, maxX, v.x), Mathf.Lerp(minY, maxY, (v.y)));
        //print(new Vector2(minX, minY).ToString("n2"));
        //print(new Vector2(maxX, maxY).ToString("n2"));
        //print(v.ToString("n2"));
        //print("........................");




        /*if (mousePosition != Input.mousePosition)
        {
            mousePosition = Input.mousePosition;
            offset = v;

            print(".");
        }*/
        
        pivot = v;
        //mp = v;

        //map.pivot = v;
        //map.anchoredPosition = new Vector2((map.rect.width * map.pivot.x) - map.rect.width / 2, (map.rect.height * map.pivot.y) - map.rect.height / 2);
        map.localScale = Vector2.one * currentZoom;
        //1280x720
        Vector2 p = new Vector2(1280, 720);
        contentArea.sizeDelta = p * currentZoom;

        //offset = (-contentArea.sizeDelta * (v - a));
        //print(offset.ToString("n3"));
        //mp.Set(Mathf.Lerp(-1, 1, v.x * 2), Mathf.Lerp(-1, 1, v.y * 2));
        //print(v.ToString("n2"));
        //print(mp.ToString("n2"));
        //mp.Scale(p*-1);
        //contentArea.anchoredPosition += mp;

        Vector2 fixedPivot = new Vector2(v.x, Mathf.Abs(v.y - 1));
        fixedPivot *= contentArea.sizeDelta;
        //fixedPivot -= p;
        Vector2 scrP = new Vector2(mp.x, Mathf.Abs(mp.y - 1)) * p;

        //print(fixedPivot);

        contentArea.anchoredPosition = ((fixedPivot * new Vector2(-1, 1))/* + scrP*/);

        //contentArea.anchoredPosition = new Vector2(((contentArea.rect.width - p.x) * -mp.x) - offset.x, ((contentArea.rect.height - p.y) * Mathf.Abs(mp.y - 1)) - offset.x);
        yield return null;
    }

    public void TestPivot(Vector2 v)
    {
        //Vector2 p = new Vector2(1280, 720);

        StartCoroutine(setPivot(v));

        //print(v);
        //print(contentArea.anchoredPosition / (p * currentZoom));
    }

    IEnumerator setPivot(Vector2 value)
    {
        yield return null;

        pivot = value;
    }
}
