using System.Collections;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using WindowsInput;
using WindowsInput.Native;

public class KeySimulator : MonoBehaviour
{
    private InputSimulator sim;
    private bool simulate;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void SetUP()
    {
        Process[] processes = Process.GetProcessesByName("DMT");
        Debug.Log(processes.Length);
    }

    private void Awake()
    {
        sim = new InputSimulator();
    }

    IEnumerator ConstantCall()
    {
        while (simulate)
        {
            yield return new WaitForSeconds(1);
            sim.Mouse.MoveMouseBy(-5, 0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //simulate = !simulate;
            //if(simulate) StartCoroutine(ConstantCall());
            //sim.Keyboard.KeyPress(VirtualKeyCode.VK_K);
            /*sim.Keyboard.KeyDown(VirtualKeyCode.LWIN)
                .KeyDown(VirtualKeyCode.LMENU)
                .KeyPress(VirtualKeyCode.F1)
                .KeyUp(VirtualKeyCode.LWIN)
                .KeyUp(VirtualKeyCode.LMENU);*/
            //sim.Mouse.MoveMouseBy(2, 2);
        }

        if (Input.GetKeyDown(KeyCode.K)) Debug.Log("key down");
        if (Input.GetKey(KeyCode.K)) Debug.Log("key");
        if (Input.GetKeyUp(KeyCode.K)) Debug.Log("key up");
    }
}
