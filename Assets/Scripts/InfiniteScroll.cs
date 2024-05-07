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
    private float topPadding; //space used by padding; FOR NOW JUST USING ONE, BUT NEED TWO FOR EACH ONE
    private float bottomPadding;
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

        totalInstances += 3; //initial plus two extras to guarantee no empty space

        Vector2 contentSize = Vector2.zero;
        contentSize.x = scrollRect.content.sizeDelta.x;
        contentSize.y = padding + ((height + spacing) * totalInstances);
        scrollRect.content.sizeDelta = contentSize;

        topPadding = layout.padding.top / (contentSize.y - totalSize) * 2;//estimate for current size: 0.05998791f at 333,4005 with 20 padding

        bottomPadding = (totalHeight - totalSize + padding) / totalSize;

        stepValue = (height + spacing) / (contentSize.y - totalSize);

        Debug.Log($"totalSize: {(contentSize.y - totalSize)} | topPadding: {topPadding}");
        Debug.Log($"totalSize: {totalSize} | bottomPadding: {bottomPadding}");

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

        float offset = position.y - topPadding;
        float point = offset / stepValue;
        int topValue = Mathf.CeilToInt(point);//upper limit is 3, lower is 1
        debugText.text = $"{topValue}\n{delta}";

        if (topValue <= 1 && delta < 0)
        {
            //seamlessly scroll back up
            firstDataID -= topValue - 2;
            float newPos = (((1 - (point - topValue)) * stepValue) - topPadding);
            lastPosition = Vector2.up * (newPos * totalSize);
            StartCoroutine(InfiniteEffect(Mathf.Abs(newPos - 1), scrollRect.velocity));
            return;
        }
        else if(delta > 0)
        {
            offset = position.y;
            point = offset / stepValue;
            topValue = Mathf.CeilToInt(point);
            debugText.text = $"{topValue}\n{delta}";
            if (firstDataID > 0 && topValue >= 3)
            {
                //seamlessly scroll back down
                firstDataID -= (topValue - 2);
                firstDataID = Mathf.Max(firstDataID, 0);

                float newPos = (((1 - (point - topValue)) * stepValue));
                lastPosition = Vector2.up * (newPos * totalSize);
                StartCoroutine(InfiniteEffect(Mathf.Abs(newPos - 1), scrollRect.velocity));
                return;
            }
        }
        lastPosition = position;
    }

    IEnumerator InfiniteEffect(float position, Vector2 velocity)
    {
        bool holdingMouse = EventSystem.current.currentInputModule.input.GetMouseButton(0);

        PointerEventData evt = new PointerEventData(EventSystem.current);
        if (holdingMouse) scrollRect.OnEndDrag(evt);

        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = position;
        scrollRect.velocity = velocity;

        if(holdingMouse)
        {
            evt.button = PointerEventData.InputButton.Left;
            evt.position = EventSystem.current.currentInputModule.input.mousePosition;

            scrollRect.OnBeginDrag(evt);
        }

        FillData(firstDataID);
    }
}
