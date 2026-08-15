using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearLayoutGroup : LenixSOLayoutGroup
{
    [Tooltip("direction in witch the elements will expand to")] public Vector2 direction;

    public float xDirection
    {
        get => direction.x;
        set => direction.x = value;
    }

    public float yDirection
    {
        get => direction.y;
        set => direction.y = value;
    }

    public override void AdjustElements()
    {
        RectTransform[] elements = GetEnabledElements();

        Vector2 startPosition = Vector2.zero;//for now the center of this object

        Vector2 expandDirection = direction.normalized;
        for (int i = 0; i < elements.Length; i++)
        {
            Vector2 position = startPosition + (expandDirection * offset);
            position.x += expandDirection.x * spacing * i;
            position.y += expandDirection.y * spacing * i;
            elements[i].anchoredPosition = position;
        }
    }
}
