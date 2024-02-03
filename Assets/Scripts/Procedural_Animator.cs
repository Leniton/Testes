using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

[ExecuteAlways]
public class Procedural_Animator : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] Transform sampleParent;
    [SerializeField, Range(-4.5f, 4.5f)] float yOffset;
    [SerializeField, Range(-8, 8)] public float xOffset;
    [SerializeField, Range(0, 9)] public float inputChange;
    [SerializeField, Range(1, 16)] public float timeRange;

    public Vector2 StartingPosition => new(xOffset, yOffset);

    void Update()
    {
        //startLine
        Color color = sampleParent.GetComponentInChildren<SpriteRenderer>().color;
        float step = 1f / (sampleParent.childCount-1);
        Vector2 previousPos = Vector2.right * xOffset + Vector2.up * yOffset;
        for (int i = 0; i < sampleParent.childCount; i++)
        {
            float currentTime = (step * i);
            Vector3 pos = Vector3.zero;
            pos.x = xOffset + (timeRange * currentTime);
            pos.y = yOffset + (curve.Evaluate(currentTime) * inputChange);
            sampleParent.GetChild(i).position = pos;

            previousPos.x = pos.x;
            Debug.DrawLine(previousPos, pos, color, .2f);
            previousPos = pos;
        }
    }

    public float GetInput(float time) => curve.Evaluate(time);
}

[Serializable]
public class InterpolationCurve
{
    private float StartTime;
    private float StartValue;
    private AnimationCurve curve;
    public bool ended = false;
    public static Action<InterpolationCurve> onEnd;//just for practical purposes, won't work in a realistic situation

    public InterpolationCurve(AnimationCurve _curve)
    {
        StartTime = Time.time;
        curve = _curve;
        StartValue = curve.Evaluate(0);
    }

    public float currentValue => ValueAt(Time.time - StartTime, true);

    public float currentDelta => currentValue - StartValue;

    public float ValueAt(float time, bool triggerEnd = false)
    {
        if (triggerEnd) CheckEnd(time);
        return curve.Evaluate(time);
    }

    public float DeltaAt(float time, bool triggerEnd = false) => ValueAt(time, triggerEnd) - StartValue;

    private void CheckEnd(float time) 
    {
        if (time >= 1)
        {
            onEnd?.Invoke(this);
            ended = true;
        }
    }
}