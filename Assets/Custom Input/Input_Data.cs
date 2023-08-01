using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInputData", menuName = "Input data")]
public class Input_Data : ScriptableObject
{
    public Dictionary<string,KeyCode> inputs = new Dictionary<string, KeyCode>();
}
