using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDisplayer : MonoBehaviour
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

        bit b = new bit(true);
        //b ^= 1;
        Debug.Log(b);
    }

    void Update()
    {
        return;
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

public struct bit
{
    private byte value;
    public bit(bool on = false) => value = (byte)(on ? 1 : 0);
    private static byte ToBit(byte by) => (byte)(by > 0 ? 1 : 0);

    public static implicit operator byte(bit bit) => bit.value;
    public static implicit operator int(bit bit) => bit.value;
    public static explicit operator bit(int n) => new bit(n > 0);

    public static bit operator +(bit b, byte by)
    {
        b.value = ToBit((byte)(b.value + by));
        return b;
    }
    public static bit operator -(bit b, byte by)
    {
        b.value = ToBit((byte)(b.value - by));
        return b;
    }
    public static bit operator |(bit b, byte by)
    {
        b.value = (byte)(ToBit(by) | b.value);
        return b;
    }
    public static bit operator &(bit b, byte by)
    {
        b.value = (byte)(ToBit(by) & b.value);
        return b;
    }
    public static bit operator ^(bit b, byte by)
    {
        b.value = (byte)(ToBit(by) ^ b.value);
        return b;
    }
    public static bit operator ~(bit b) => b ^ 1;
    public static bit operator !(bit b) => ~b;

    public override string ToString() => value.ToString();
}