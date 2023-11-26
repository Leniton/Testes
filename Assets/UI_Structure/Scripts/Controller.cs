using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField]View view;

    private void Awake()
    {
        view = new();
        view.Init(canvas.transform);
        view.Type(text:"testing typing texts");
    }
}
