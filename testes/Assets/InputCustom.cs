using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCustom : MonoBehaviour
{
    [SerializeField]
    bool WaitingInput;
    KeyCode[] bidings = new KeyCode[5];

    Event KeyEvent;

    public void WaitForInput()
    {
        WaitingInput = true;
    }

    void OnGUI()
    {
        KeyEvent = Event.current;
        if (KeyEvent.isKey && WaitingInput)
        {
            WaitingInput = false;
            print("tecla pressionada: " + KeyEvent.keyCode);
            bidings.SetValue(KeyEvent.keyCode, 0);
        }
    }
}
