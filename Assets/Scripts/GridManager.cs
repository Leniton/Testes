using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class GridManager : MonoBehaviour
{
    private Dictionary<Vector2, GameObject> grid = new();
    
    private static GridManager instance;

    private void Awake() => instance = this;

    public static GameObject GetElement(Vector2 point)
    {
        return instance.grid.GetValueOrDefault(point);
    }

    public static void SetElement(Vector2 point, GameObject element)
    {
        instance.grid[point] = element;
    }
}