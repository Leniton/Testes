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
            float result = MultiplyFunction(value) * yScale;
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

    private float AddFunction(float x, float a = 1)
    {
        return x + a;//f(x) = x + a
    }

    private float MultiplyFunction(float x, float m = 2)
    {
        return x * m;//f(x) = x * m
    }

    private float DivisiveFunction(float x, float d = 1)
    {
        return d / x;//f(x) = 1/x
    }
    private float ExponentialFunction(float x, float pow = 2)
    {
        return Mathf.Pow(x, pow);//f(x) = pow^x
    }

    private float RootFunction(float x)
    {
        return Mathf.Sqrt(x);
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
}