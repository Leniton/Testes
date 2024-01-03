using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    [SerializeField] private float min, max, interval;
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
            float value = (min + (interval * i));
            float result = ReverseExponentialFunction(value);
            dot.QueueAction((obj) =>
            {
                Vector2 position = new(value, result);
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
}