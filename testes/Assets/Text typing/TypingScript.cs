using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingScript : MonoBehaviour
{

    public TextToType TTT;
    [Range(0,1)] public float Pace;
    float customPace;

    [SerializeField]TextMeshProUGUI Text;

    void Start()
    {
        //testando, apagar dps
        Type(TTT);
    }

    public void Type(TextToType t)
    {
        t = TTT;
        customPace = Pace;
        StartCoroutine(Typing());
    }

    public void Type(TextToType t, float speed)
    {
        t = TTT;
        customPace = speed;
        StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        Text.text = TTT.StartText;

        int cCount = 0;

        do
        {
            Text.text += TTT.ToTypeText[cCount];

            if (customPace <= 0 || TTT.ToTypeText[cCount] == ' ')
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(customPace * Time.deltaTime);
            }

            cCount++;

        } while (cCount < TTT.ToTypeText.Length);
    }

}
