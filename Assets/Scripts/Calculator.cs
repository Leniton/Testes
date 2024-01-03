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
            float result = Function(value);
            dot.QueueAction((obj) =>
            {
                Vector2 position = new(value, result);
                obj.transform.position = position;
                dots.Add(obj);
            });
        }
    }

    private float Function(float x)
    {
        return x; //f(x) = x
    }
}