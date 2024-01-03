using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    [SerializeField] private float min, max, interval;
    [Space]
    [SerializeField] float xScale = 1;
    [SerializeField] float yScale = 1;
    private List<GameObject> dots = new();

    public void Calculate()
    {
        //clear list
        for (int i = 0; i < dots.Count; i++)
        {
            DestroyImmediate(dots[i]);
        }
        dots.Clear();

        int n = Mathf.RoundToInt((max - min) / interval);

        for (int i = 0; i <= n; i++)
        {
            AddressableAsyncObject dot = new("Dot", transform);
            float value = (min + (interval * i)) * xScale;
            float result = SineFunction(value) * yScale;
            dot.QueueAction((obj) =>
            {
                Vector2 position = new(value, result);
                obj.transform.position = position;
                dots.Add(obj);
            });
        }
    }

    public void DrawCircle()
    {
        for (int i = 0; i < dots.Count; i++)
        {
            DestroyImmediate(dots[i]);
        }
        dots.Clear();

        int n = Mathf.RoundToInt(360 / interval);
        for (int i = 0; i <= n; i++)
        {
            AddressableAsyncObject dot = new("Dot", transform);
            float value = (min + (interval * i)) * xScale;
            float resultX = SineFunction(value) * yScale;
            float resultY = CossineFunction(value) * yScale;
            dot.QueueAction((obj) =>
            {
                Vector2 position = new(resultX, resultY);
                obj.transform.position = position;
                dots.Add(obj);
            });
        }
    }

    private float EqualFunction(float x)
    {
        return x;//f(x) = x
    }

    private float ExponentialFunction(float x)
    {
        return Mathf.Pow(x, 2);//f(x) = x^2
    }

    private float ReverseExponentialFunction(float x, float pow = 2)
    {
        return Mathf.Pow(pow, x);//f(x) = pow^x
    }

    private float LogFunction(float x)
    {
        return Mathf.Log(x);//f(x) = log(x)
    }

    private float CossineFunction(float x)
    {
        return Mathf.Cos(x);
    }

    private float SineFunction(float x)
    {
        return Mathf.Sin(x);
    }

    private float TangentFunction(float x)
    {
        return Mathf.Tan(x);
    }

    private float CircleFunction(float x, float radius = 1)
    {
        return Mathf.Cos(x);
    }
}