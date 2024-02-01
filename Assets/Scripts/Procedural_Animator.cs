using UnityEngine;

[ExecuteAlways]
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

        step = 1f / (TTR_SampleParent.childCount/2);

        for (int i = 0; i < TTR_SampleParent.childCount; i++)
        {
            float time = step * i;
            //Debug.Log($"input: {time} | output: {DetermineAlphaValue(time, 0, timeBalance, 1 - timeBalance)}");
            TTR_SampleParent.GetChild(i).position = rect.min + (Vector2.right * rect.max * time) + Vector2.up * (GetY(time));
        }
    }

    float GetX(float time)
    {
        //Debug.Log($"{time} | {changeTime}");
        return inputChange * Mathf.Clamp01(DetermineAlphaValue(time, changeTime, 99999, timeStep));
    }

    float GetY(float time)
    {
        time = time-1 == 0 ? .01f : time - 1;
        //Debug.Log(time);
        float y = inputChange * Mathf.Clamp01(DetermineAlphaValue(time, changeTime, 99999, timeStep));

        return y;
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


public struct InterpolationCurve
{
    //values used in movement
    float currentValue;
    float currentSpeed;
    float currentAceleration;

    //values used for changing the linear movement
    float _acelerationRate;

    public InterpolationCurve(float startValue, float endValue, float acelerationRate = 0)
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