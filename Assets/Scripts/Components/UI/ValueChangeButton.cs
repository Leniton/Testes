using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ValueChangeButton : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] protected ValueInputField inputField;
    [SerializeField] protected int Value;

    protected void Awake()
    {
        button.onClick.AddListener(ChangeValue);
    }

    public abstract void ChangeValue();
}
