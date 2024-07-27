using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Debug = UnityEngine.Debug;

public class KeySimulator : MonoBehaviour
{

    private bool simulate;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void SetUP()
    {
        Process[] processes = Process.GetProcessesByName("DMT");
        Debug.Log(processes.Length);
        
    }

    private void Start()
    {
        
    }

    IEnumerator ConstantCall()
    {
        while (simulate)
        {
            yield return new WaitForSeconds(1);
            SimulateInput.Main.SimulateInput("");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            simulate = !simulate;
            if(simulate) StartCoroutine(ConstantCall());
        }

        if (Input.GetKeyDown(KeyCode.K)) Debug.Log("key down");
        if (Input.GetKey(KeyCode.K)) Debug.Log("key");
        if (Input.GetKeyUp(KeyCode.K)) Debug.Log("key up");
    }
}
