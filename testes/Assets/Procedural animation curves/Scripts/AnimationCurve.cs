using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurve : MonoBehaviour
{
    [SerializeField] Transform movingObject;
    [SerializeField] float startValue, endvalue, acelerationRate;
    InterpolationCurve curve;
    [Header("Time values")]
    [SerializeField] float duration;
    float currentTime = 0;

    Vector3 initialPos;

    void Start()
    {
        initialPos = movingObject.position;
        curve = new InterpolationCurve(startValue, endvalue, acelerationRate);
    }

    void Update()
    {
        if (currentTime <= duration)
        {
            currentTime += Time.deltaTime;
            movingObject.position = initialPos + Vector3.right * curve.Step(Time.deltaTime);
        }
        else
        {
            currentTime = 0;
            curve = new InterpolationCurve(startValue, endvalue, acelerationRate);
        }
    }
}

public struct InterpolationCurve
{
    //values used in movement
    float currentValue;
    float currentSpeed;
    float currentAceleration;

    //values used for changing the linear movement
    float _acelerationRate;

    public InterpolationCurve(float startValue, float endValue,float acelerationRate=0)
    {
        _acelerationRate = acelerationRate;

        currentValue = startValue;
        currentSpeed = endValue - startValue;
        currentAceleration = currentSpeed * _acelerationRate;
    }

    public float Step(float deltaTime)
    {
        currentSpeed += currentAceleration * deltaTime;
        currentValue += currentSpeed * deltaTime;
        return currentValue;
    }
}