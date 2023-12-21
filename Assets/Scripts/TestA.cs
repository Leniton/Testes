using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Random = UnityEngine.Random;

public class TestA : MonoBehaviour
{
    [SerializeField] private string a;
    private void PrivateNoParam()
    {
        Debug.Log("invoking privateNoParam method");
    }
    [SerializeMethod]
    int WithReturnType()
    {
        //Debug.Log("invoking WithReturnType method");
        return Random.Range(0,10);
    }

    public void WithParams(string s, float f = 0, int i = 2, bool b = false)
    {
        Debug.Log($"invoking WithParams method.\n params: i={i} | n={f} | s=\"{s}\" | b={b}");
    }
}
