using Newtonsoft.Json.Linq;
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
    [Space]
    [Header("TimeToRespond")]
    [SerializeField] Transform TTR_SampleParent;
    [SerializeField, Range(0, 1)] float timeToReact;
    [Range(.1f, .9f)] public float timeBalance;
    [Space]
    [Header("InitialResponse")]
    [SerializeField] Transform IR_SampleParent;
    [SerializeField, Range(-1, 1)] float initialResponse;
    [Space]
    [Header("DampEffect")]
    [SerializeField] Transform DE_SampleParent;
    [SerializeField, Range(0, 2)] float dampEffect;
    [Space]
    [Header("InitialResponse")]
    [SerializeField] Transform FinalAnimationParent;

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
        rect = new Rect(0, yOffset, timeRange, inputChange);
        step = 1f / (TTR_SampleParent.childCount-1);

        for (int i = 0; i < TTR_SampleParent.childCount; i++)
        {
            float time = step * i;
            float currentTime = ((time / timeBalance) / 2) * (DetermineAlphaValue(time,0,timeBalance,1-timeBalance)/2);
            currentTime += ((Mathf.Clamp((time) - timeBalance, 0, 99999999 - timeBalance) / (1 - timeBalance)) / 2) *
                (DetermineAlphaValue(time, 1, 99999999, 1 - timeBalance) / 2);
            //Debug.Log($"input: {time} | output: {DetermineAlphaValue(time, 0, timeBalance, 1 - timeBalance)}");
            TTR_SampleParent.GetChild(i).position = rect.min + Vector2.right * (rect.width * TimeToReact(time)) +
                Vector2.up * (rect.height * currentTime);
        }

        //initial Response: amplitude of sine wave based on time to respond value
        //line code for initial response
        step = 1f / IR_SampleParent.childCount;
        for (int i = 0; i < IR_SampleParent.childCount; i++)
        {
            IR_SampleParent.GetChild(i).position = rect.min + Vector2.right * (rect.width * TimeToReact(step * i)) +
                Vector2.up * (InitialResponse(initialResponse, step * i));
        }
        //damp value: drag; value that reduces the initial response amplitude over time. proportional to initial response
        //line code for damp value(?)
        step = (1f / DE_SampleParent.childCount) * 2;
        for (int i = 0; i < DE_SampleParent.childCount; i++)
        {

            DE_SampleParent.GetChild(i).position = rect.min + Vector2.right * (rect.width * TimeToReact(step * i)) +
                Vector2.up * (InitialResponse(DampEffect(initialResponse, step * i, dampEffect), step * i));
        }
        //adding modifiers in a single sample
        step = (1f / FinalAnimationParent.childCount) * 4;
        for (int i = 0; i < FinalAnimationParent.childCount; i++)
        {
            float time = step * i;
            FinalAnimationParent.GetChild(i).position = rect.min + Vector2.right * (rect.width * TimeToReact(time)) +
                Vector2.up * (FullAnimation(time));
        }
    }

    public float TimeToReact(float time)
    {
        float currentTime = time;
        return currentTime * timeToReact;
    }

    float InitialResponse(float amp, float time, float frequency = .5f)
    {
        return inputChange * amp * Mathf.Sin(2 * Mathf.PI * frequency/*frequency*/ * time);
    }

    float DampEffect(float delta,float time,float damp = 1)
    {
        float max = Mathf.Max(delta, 0);
        float min = Mathf.Min(delta, 0);
        return Mathf.Clamp(delta * (1 - (damp) * time), min, max);
    }

    public float FullAnimation(float time)
    {
        float delta = inputChange * (Mathf.Clamp01(time));
        delta += InitialResponse(DampEffect(initialResponse, time, dampEffect), time);
        return delta;
    }

    public Vector2 initialPos()
    {
        return new Vector2(0, yOffset);
    }


    float NormalizedDistance(float value, float target, float range)
    {
        return Mathf.Clamp01(1 - Mathf.Abs(value - target) / range);
    }
    float WithinTargetDistance(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
    float DetermineAlphaValue(float value, float min, float max, float range)
    {
        float alphaValue = NormalizedDistance(value, min, range) +
                Mathf.Clamp01(NormalizedDistance(value, max, range) +
                Mathf.Ceil(NormalizedDistance(value, min + ((max - min) / 2), (max - min) / 2))) *
                Mathf.Ceil(Mathf.Clamp01(WithinTargetDistance(value, min, max)));
        return alphaValue;
    }


}
