using System;
using System.Collections.Generic;
using GridSystem;
using LenixSO.Sequences.Coroutines;
using LenixSO.Sequences.Decorator;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class GridManager : MonoBehaviour, IGrid
{
    private Dictionary<Vector2, GameObject> grid = new();
    private Vector2 offset;
    
    private static GridManager instance;

    public int Width { get; } = 17;
    public int Height { get; } = 9;
    public List<ITile> tiles { get; set; } = new();
    public ITile hoveredTile { get; set; } = null;
    public bool currentlySelecting { get; set; }
    public Action<ITile> onClick { get; set; }
    public Action<ITile> onEnter { get; set; }
    public Action<ITile> onExit { get; set; }
    
    private void Awake()
    {
        instance = this;
        IGrid.Instance = this;
        SetUpGrid();
    }
    
    public void SetUpGrid()
    {
        var grid = this as IGrid;
        offset = new(Width / 2, Height / 2);
        // offset = Vector2.zero;
        int size = Width * Height;
        for (int i = 0; i < size; i++)
        {
            var tile = new DebugTile(Vector2.zero);
            tiles.Add(tile);
            tile.origin = grid.GetTileCoordinates(tile) - offset;
        }
    }

    public ITile GetTileAt(Coordinate coordinates)
    {
        coordinates += offset;
        return (coordinates.x < 0 || coordinates.y < 0 || coordinates.x >= Width || coordinates.y >= Height)
                ? null
                : tiles[(Width * coordinates.y) + coordinates.x];
    }

    public static GameObject GetElement(Vector2 point)
    {
        return instance.grid.GetValueOrDefault(point);
    }

    public static void SetElement(Vector2 point, GameObject element)
    {
        instance.grid[point] = element;
    }
}

public class DebugTile : ITile
{
    public List<IPiece> pieces { get; set; } = new();
    public Color defaultColor { get; private set; } = Color.white.Transparent(.02f);
    public Color selectableColor { get; }
    public Color validColor { get; }
    public Color invalidColor { get; }
    public ITile.State state { get; set; }
    public List<Color> colors { get; set; } = new();
    public Action<ITile> onClick { get; set; }
    public Action<ITile> onEnter { get; set; }
    public Action<ITile> onExit { get; set; }

    private float size;
    public Vector2 origin;

    public DebugTile(Vector2 center, float size = 1)
    {
        origin = center;
        this.size = size;
        new LoopingSequence(
            new SetupSequence(DrawTile,
                new CoroutineSequence(
                    new(CoroutineExtensions.DelayCoroutine(.2f))
                    )
                )
            ).Begin();
    }

    private void DrawTile()
    {
        float offset = size / 2f;
        Debug.DrawLine(new(origin.x - offset, origin.y - offset), new(origin.x - offset, origin.y + offset), defaultColor, .2f);
        Debug.DrawLine(new(origin.x - offset, origin.y + offset), new(origin.x + offset, origin.y + offset), defaultColor, .2f);
        Debug.DrawLine(new(origin.x + offset, origin.y + offset), new(origin.x + offset, origin.y - offset), defaultColor, .2f);
        Debug.DrawLine(new(origin.x + offset, origin.y - offset), new(origin.x - offset, origin.y - offset), defaultColor, .2f);
    }
    
    public void ApplyColor(Color color)
    {
        defaultColor = color;
    }
}