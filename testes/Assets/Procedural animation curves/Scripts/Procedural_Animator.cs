using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Procedural_Animator : MonoBehaviour
{
    [SerializeField]Transform sampleParent;
    [SerializeField, Range(-3, 0)] float yOffset;
    [SerializeField, Range(0, 6)] float inputChange;
    [SerializeField, Range(0, 7)] float timeRange;

    void Update()
    {
        //startLine
        Rect rect = new Rect(-timeRange, yOffset, timeRange * 2, inputChange);
        int sampleDivision = sampleParent.childCount / 3;
        float step = 1f / sampleDivision;

        for (int i = 0; i < sampleDivision; i++)
        {
            //code for initial straight line
            sampleParent.GetChild(i).position = rect.min + (Vector2.right * rect.max * step * i);
        }

        for (int i = sampleDivision; i < sampleDivision*2; i++)
        {
            //code for straight line towards change in input
            sampleParent.GetChild(i).position = Vector2.up * (rect.min) +
                Vector2.up * (rect.height * step * (i - sampleDivision));
        }

        for (int i = sampleDivision * 2; i < sampleDivision * 3; i++)
        {
            //code for final straight line

            sampleParent.GetChild(i).position = Vector2.right * (rect.max * step * (i - (sampleDivision * 2))) +
                Vector2.up * (rect.max);
        }

        //!!! all values are proportional to inputChange

        //time to resepond: lerp rate for initial response center
        //line code for timeTo respond

        //initial Response: amplitude of sine wave based on time to respond value
        //line code for initial response

        //damp value: drag; value that reduces the initial response amplitude over time. proportional to initial response
        //line code for damp value(?)

    }
}
