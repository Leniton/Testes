using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[SerializeMethods]
public class TestA : MonoBehaviour
{
    [SerializeField] private string a;
    private void PrivateNoParam()
    {
        Debug.Log("invoking privateNoParam method");
    }

    int WithReturnType()
    {
        Debug.Log("invoking WithReturnType method");
        return 1;
    }

    public void WithParams(string s, float f = 0,int i=2,bool b = false)
    {
        Debug.Log($"invoking WithParams method. params: i={i} | n={f} | s=\"{s}\" | b={b}");
    }
}
