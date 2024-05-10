using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    [SerializeField] private DataInstancer instancer;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private VerticalLayoutGroup layout;
    private Scrollbar scrollbar;
    [Space]
    [SerializeField] private float padding;
    [SerializeField] private float spacing;

    private float totalSize;
    private float stepValue;
    private int totalInstances;
    private int firstDataID = 0;
    private float bottomPadding;
    float overshoot;
    float undershoot;

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
        scrollbar = scrollRect.verticalScrollbar;
        scrollRect.verticalScrollbar = null;
        scrollbar.gameObject.SetActive(true);

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

        totalInstances += 5; //initial plus 5 extras to guarantee no empty space

        Vector2 contentSize = Vector2.zero;
        contentSize.x = scrollRect.content.sizeDelta.x;
        contentSize.y = padding + ((height + spacing) * totalInstances);
        scrollRect.content.sizeDelta = contentSize;

        float totalMovement = (contentSize.y - totalSize);

        overshoot = (height - (totalHeight - totalSize)) / totalMovement;
        undershoot = (totalHeight - totalSize) / totalMovement;

        stepValue = (height + spacing) / totalMovement;

        bottomPadding = layout.padding.bottom / totalMovement;

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
        scrollbar.size = totalInstances / (float)(startingData + totalInstances);
    }

    private void OnScrollMoved(Vector2 position)
    {
        float delta = position.y - lastPosition.y;

        float offset = position.y - bottomPadding;
        float point = offset / stepValue;
        int topValue = Mathf.CeilToInt(point);

        if (topValue <= 1 && delta < 0)
        {
            //seamlessly scroll back up
            firstDataID -= topValue - 3;

            float newPos = ((1+point) * stepValue) - (bottomPadding + overshoot);
            lastPosition = Vector2.up * (newPos * totalSize);
            StartCoroutine(InfiniteEffect(Mathf.Abs(newPos - 1), scrollRect.velocity));
            return;
        }
        else if(delta > 0)
        {
            offset = position.y - undershoot;
            point = offset / stepValue;
            topValue = Mathf.CeilToInt(point);
            if (firstDataID > 0 && topValue >= 4)
            {
                //seamlessly scroll back down
                firstDataID -= topValue - 3;
                firstDataID = Mathf.Max(firstDataID, 0);

                float newPos = ((point) * stepValue) - (undershoot + overshoot);
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
