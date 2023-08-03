using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Procedural_Animator : MonoBehaviour
{
    [SerializeField] Transform sampleParent;
    [SerializeField, Range(-3, 0)] float yOffset;
    [SerializeField, Range(-5, 0)] float xOffset;
    [SerializeField, Range(0, 6)] float inputChange;
    [SerializeField, Range(1, 14)] int changeID;
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
    float timeStep => (1f / (sampleParent.childCount / 3f));
    float changeTime => timeStep * changeID;
    void Update()
    {
        //startLine
        Rect rect = new Rect(xOffset, yOffset, timeRange - xOffset, inputChange);
        int sampleDivision = sampleParent.childCount / 3;
        float step = 1f / sampleDivision;
        sampleDivision *= 2;
        for (int i = 0; i < sampleDivision; i++)
        {
            //code for initial straight line
            sampleParent.GetChild(i).position = rect.min + (Vector2.right * rect.max * step * i) +
                Vector2.up*(GetX(i * step));
        }
        step = 1f / (sampleParent.childCount - sampleDivision-1);
        for (int i = sampleDivision; i < sampleParent.childCount; i++)
        {
            //code for straight line towards change in input
            sampleParent.GetChild(i).position = 
                sampleParent.GetChild(Mathf.Clamp(changeID - 1,0,sampleParent.childCount)).position +
                Vector3.up * ((rect.height) * step * (i - sampleDivision));
        }

        //!!! all values are proportional to inputChange

        //time to resepond: lerp rate for initial response center
        //line code for timeTo respond
        step = 1f / (TTR_SampleParent.childCount/2);

        for (int i = 0; i < TTR_SampleParent.childCount; i++)
        {
            float time = step * i;
            //Debug.Log($"input: {time} | output: {DetermineAlphaValue(time, 0, timeBalance, 1 - timeBalance)}");
            TTR_SampleParent.GetChild(i).position = rect.min + (Vector2.right * rect.max * time) + Vector2.up * (GetY(time));
        }

        return;
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

    float GetX(float time)
    {
        //Debug.Log($"{time} | {changeTime}");
        return inputChange * Mathf.Clamp01(DetermineAlphaValue(time, changeTime, 99999, timeStep));
    }

    float GetY(float time)
    {
        float y = GetX(time);

        return y;
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
