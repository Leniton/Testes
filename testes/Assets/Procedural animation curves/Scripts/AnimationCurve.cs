using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurve : MonoBehaviour
{
    [SerializeField] Transform movingObject;
    [SerializeField] Procedural_Animator animator;
    [Header("Time values")]
    [SerializeField] float duration,timeModifier = 1,timeRange = 1;
    float currentTime = 0;

    Vector3 initialPos;

    void Start()
    {
        initialPos = animator.initialPos();
    }

    void Update()
    {
        if (currentTime <= duration)
        {
            currentTime += Time.deltaTime * timeModifier;
            movingObject.position = initialPos + (Vector3.right * animator.TimeToReact(currentTime)*timeRange) +
                Vector3.up * animator.FullAnimation(currentTime);
        }
        else
        {
            currentTime = 0;
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