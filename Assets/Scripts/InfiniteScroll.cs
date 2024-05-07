using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    //DEBUG ONLY
    [SerializeField] TMPro.TMP_Text debugText;

    [SerializeField] private DataInstancer instancer;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] VerticalLayoutGroup layout;
    [Space]
    [SerializeField] private float padding;
    [SerializeField] private float spacing;

    private float totalSize;
    private float stepValue;
    private float deadZone; //space used by padding; FOR NOW JUST USING ONE, BUT NEED TWO FOR EACH ONE
    private int totalInstances;
    private int firstDataID = 0;
    private int lastDataID = 0;

    private Vector2 lastPosition = Vector2.one;

    List<DataObject> dataInstances = new();

    private void Awake()
    {
        if(!scrollRect) scrollRect = GetComponent<ScrollRect>();
        StartCoroutine(SetUp());
    }

    private IEnumerator SetUp()
    {
        yield return new WaitForSeconds(.4f);

        scrollRect.onValueChanged.AddListener(OnScrollMoved);

        //fill viewable scroll
        totalSize = scrollRect.viewport.rect.height;
        float height = instancer.dataHeight;
        float spacing = layout.spacing;
        float padding = layout.padding.top + layout.padding.bottom;

        float totalHeight = padding + height;

        while (totalHeight < totalSize)
        {
            totalInstances++;
            totalHeight += height + spacing;
        }

        totalInstances += 3; //initial plus two extra to guarantee no empty space

        Vector2 contentSize = Vector2.zero;
        contentSize.x = scrollRect.content.sizeDelta.x;
        contentSize.y = padding + ((height + spacing) * totalInstances);
        scrollRect.content.sizeDelta = contentSize;

        deadZone = padding / (contentSize.y - totalSize); //estimate for current size: 0.2f
        stepValue = (height + spacing) / (contentSize.y - totalSize);
        Debug.Log(deadZone);

        FillData(0);
    }

    private void FillData(int startingData)
    {
        firstDataID = startingData;
        for (int i = 0; i < totalInstances; i++)
        {
            Data data = instancer.GetData(startingData + i);
            if (dataInstances.Count < i + 1)
            {
                dataInstances.Add(instancer.GetInstance(data));
                dataInstances[i].transform.SetParent(scrollRect.content);
            }
            dataInstances[i].SetData(data);
        }
        lastDataID = totalInstances - 1;
    }

    private void OnScrollMoved(Vector2 position)
    {
        float delta = position.y - lastPosition.y;

        float offset = position.y - deadZone;
        float point = offset / stepValue;
        int topValue = Mathf.CeilToInt(point);//upper limit is 2, lower is 2
        debugText.text = $"{topValue}\n{delta}";

        if (topValue <= 1 && delta <= 0)
        {
            //seamlessly scroll back up
            float newPos = (((1 - (point - topValue)) * stepValue) - deadZone);
            //Debug.Log($"loop: {position.y} | {newPos}");
            //scrollRect.content.anchoredPosition = Vector2.up * newPos;
            lastPosition = Vector2.up * (newPos * totalSize);
            firstDataID += -(topValue - 2);
            StartCoroutine(InfiniteEffect(Mathf.Abs(newPos - 1), scrollRect.velocity));
            return;
        }
        else if(topValue <= 0)
        {
            debugText.text = $"pos: {position}\ndelta: {delta}\ntopValue: {topValue}";
        }
        lastPosition = position;
    }

    IEnumerator InfiniteEffect(float position, Vector2 velocity)
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = position;
        scrollRect.velocity = velocity;

        FillData(firstDataID);
    }
}
