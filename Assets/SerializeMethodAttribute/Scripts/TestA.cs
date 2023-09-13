using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class TestA : MonoBehaviour
{
    [SerializeField] private string a;
    private void PrivateNoParam(string a)
    {
        Debug.Log("invoking privateNoParam method");
    }
    [SerializeMethod]
    int WithReturnType()
    {
        Debug.Log("invoking WithReturnType method");
        return 1;
    }

    public void WithParams(string s, float f = 0, int i = 2, bool b = false)
    {
        Debug.Log($"invoking WithParams method.\n params: i={i} | n={f} | s=\"{s}\" | b={b}");
    }
}
