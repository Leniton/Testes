using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTextSequence", menuName = "Text sequence")]
public class TextSequence : ScriptableObject
{
    public TextToType[] Sequence;
}
