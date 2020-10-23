using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom_Input : MonoBehaviour
{
    //326
    [SerializeField] List<string> inputName;
    [SerializeField] List<KeyCode> inputButton;
    public Dictionary<string, KeyCode> inputs;
    public KeyCode jumpkey;
    public bool checking;

    /*void Start()
    {
        print((int)KeyCode.A);
    }*/

    void Update()
    {
        if (checking)
        {
            if (Input.anyKeyDown)
            {
                checkButton();
            }
        }
    }

    void checkButton()
    {
        KeyCode pressed;
        for (int i = 0; i < 326; i++)
        {
            if (Input.GetKeyDown((KeyCode)i)){
                print((KeyCode)i);
            }
        }
    }
}
