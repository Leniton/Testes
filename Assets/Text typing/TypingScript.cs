using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingScript : MonoBehaviour
{
    public TextSequence sequence;
    int currentTextID = 0;
    public TextToType TTT;
    [Range(0, 0.3f)] public float Pace;
    float customPace;
    bool skip = false;
    bool doneTyping = false;

    [SerializeField]TextMeshProUGUI Text;

    void Start()
    {
        //testando, apagar dps
        
        Type(sequence);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skip = true;
            if (doneTyping)
            {
                if (currentTextID < sequence.Sequence.Length - 1)
                {
                    skip = false;
                    NextInSequence();
                }
                else
                {
                    skip = false;
                    Text.text = string.Empty;
                }
            }
        }
    }

    public void Type(TextToType t)
    {
        TTT = t;
        customPace = Pace;
        StartCoroutine(Typing());
    }

    public void Type(TextToType t, float speed)
    {
        TTT = t;
        customPace = speed;
        StartCoroutine(Typing());
    }

    public void Type(TextSequence t)
    {
        sequence = t;
        currentTextID = 0;
        TTT = t.Sequence[0];

        customPace = Pace;
        StartCoroutine(Typing());
    }

    public void Type(TextSequence t, float speed)
    {
        sequence = t;
        currentTextID = 0;

        TTT = t.Sequence[0];
        customPace = speed;
        StartCoroutine(Typing());
    }

    void NextInSequence()
    {
        currentTextID++;
        TTT = sequence.Sequence[currentTextID];
        StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        doneTyping = false;
        Text.text = TTT.StartText;

        int cCount = 0;

        do
        {
            Text.text += TTT.ToTypeText[cCount];

            if (skip)
            {
                Text.text = TTT.StartText + TTT.ToTypeText;
                cCount = TTT.ToTypeText.Length;
                skip = false;
            }
            else
            {
                if (customPace <= 0 || TTT.ToTypeText[cCount] == ' ')
                {
                    yield return null;
                }
                else
                {
                    yield return new WaitForSeconds(customPace);
                }
            }
            

            cCount++;

        } while (cCount < TTT.ToTypeText.Length);
        doneTyping = true;
    }

}
