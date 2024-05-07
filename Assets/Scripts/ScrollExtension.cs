using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollExtension : MonoBehaviour
{
    [SerializeField] float edgeTreshold;
    private ScrollRect scroll;

    public Action onReachEnd, onReachStart;

    private void Awake()
    {
        scroll = GetComponent<ScrollRect>();
        scroll.onValueChanged.AddListener(ScrollMoved);
    }

    private void ScrollMoved(Vector2 value)
    {
        //Debug.Log($"value: {value}");
        //0 is end, 1 is start
    }
}
