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
        //Debug.Log(processes.Length);
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
            //sim.Mouse.MoveMouseBy(-5, 0);
            sim.Keyboard.KeyPress(VirtualKeyCode.VK_K);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            simulate = !simulate;
            if(simulate) StartCoroutine(ConstantCall());
            //sim.Keyboard.KeyPress(VirtualKeyCode.VK_K);
            /*sim.Keyboard.KeyDown(VirtualKeyCode.LWIN)
                .KeyDown(VirtualKeyCode.LMENU)
                .KeyPress(VirtualKeyCode.F1)
                .KeyUp(VirtualKeyCode.LWIN)
                .KeyUp(VirtualKeyCode.LMENU);*/
            //sim.Mouse.MoveMouseBy(2, 2);\

            /*ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "\"C:\\Users\\leniton.carneiro\\AppData\\Local\\Programs\\Opera GX\\launcher.exe\"";
            startInfo.WorkingDirectory = string.Empty;
            startInfo.UseShellExecute = false;
            startInfo.Arguments = " --side-profile-name=31343533325F31313537303035323933 --side-profile-minimal --side-profile-clear-on-exit --side-profile-muted --side-profile-no-gx-sounds --with-feature:side-profiles --private";
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();*/
        }

        //if (Input.GetKeyDown(KeyCode.K)) Debug.Log("key down");
        //if (Input.GetKeyDown(KeyCode.K)) sim.Keyboard.KeyPress(VirtualKeyCode.VK_M);
        //if (sim.InputDeviceState.IsKeyDown(VirtualKeyCode.VK_M)) Debug.Log("key? down");
        //if (Input.GetKeyDown(KeyCode.M)) Debug.Log("key down");
        //if (Input.GetKey(KeyCode.M)) Debug.Log("key");
        //if (Input.GetKeyUp(KeyCode.M)) Debug.Log("key up");
    }
}
