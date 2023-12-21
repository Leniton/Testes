using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTextToType", menuName = "Text to type")]
public class TextToType : ScriptableObject
{
    [Multiline] public string StartText;
    [TextArea] public string ToTypeText;
}
