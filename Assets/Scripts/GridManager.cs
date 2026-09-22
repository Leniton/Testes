using System;
using System.Collections.Generic;
using GameData;
using GridSystem;
using LenixSO.Sequences.Coroutines;
using LenixSO.Sequences.Decorator;
using UnityEngine;

[DefaultExecutionOrder(-99)]
public class GridManager : MonoBehaviour, IGrid
{
    private Vector2 offset;

    public int Width { get; } = 17;
    public int Height { get; } = 9;
    public List<ITile> tiles { get; set; } = new();
    public ITile hoveredTile { get; set; } = null;
    public bool currentlySelecting { get; set; }
    public SelectData selectData { get; set; }
    
    public Action<ITile> onClick { get; set; }
    public Action<ITile> onEnter { get; set; }
    public Action<ITile> onExit { get; set; }
    
    private void Awake()
    {
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
        
        //Debug barricades
        
        IPiece b = new PlayerInput.ObjectPiece(new("barricade"));
        b.Initialize();
        var material = new MaterialTrait();
        material.RegisterCallback(MaterialTrait.ExposureType.Heat, () =>
        {
            if (material.state == MaterialTrait.State.Solid)
                material.ChangeState(MaterialTrait.State.Dust, MaterialTrait.ExposureType.Heat);
            else if (material.state == MaterialTrait.State.Dust)
                material.ChangeState(MaterialTrait.State.Gas, MaterialTrait.ExposureType.Heat);
        });
        b.AddTrait(material);
        Coordinate c = new Coordinate(0, 1);
        IPiece.PlacePieceOnTile(b, GetTileAt(c), c);
        
        b = new PlayerInput.ObjectPiece(new("barricade"));
        b.Initialize();
        b.AddTrait(new MovableTrait());
        c = new Coordinate(0, -1);
        IPiece.PlacePieceOnTile(b, GetTileAt(c), c);
        
        b = new PlayerInput.ObjectPiece(new("barricade"));
        b.Initialize();
        c = new Coordinate(1, 0);
        IPiece.PlacePieceOnTile(b, GetTileAt(c), c);
        
        // b = new PlayerInput.ObjectPiece(new("barricade"));
        // b.Initialize();
        // c = new Coordinate(-1, 0);
        // IPiece.PlacePieceOnTile(b, GetTileAt(c), c);
    }

    public ITile GetTileAt(Coordinate coordinates)
    {
        coordinates += offset;
        return (coordinates.x < 0 || coordinates.y < 0 || coordinates.x >= Width || coordinates.y >= Height)
                ? null
                : tiles[(Width * coordinates.y) + coordinates.x];
    }
}

public class DebugTile : ITile
{
    public List<IPiece> pieces { get; set; } = new();
    public Color defaultColor { get; private set; } = Color.white.Transparent(.02f);
    public Color selectableColor { get; } = Color.red.Transparent(.2f);
    public Color validColor { get; }
    public Color invalidColor { get; }
    public ITile.State state { get; set; }
    public List<Color> colors { get; set; } = new();
    public Action<ITile> onPickTile { get; set; }
    public Action<ITile> onSelectionEnter { get; set; }
    public Action<ITile> onSelectionExit { get; set; }
    public Action<IPiece> onPiecePlaced { get; set; }
    public Action<IPiece> onPieceRemoved { get; set; }
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
        var tile = this as ITile;
        Color color = (tile.pieceID & 1) == 0 ? selectableColor : defaultColor;
        float offset = size / 2f;
        Debug.DrawLine(new(origin.x - offset, origin.y - offset), new(origin.x - offset, origin.y + offset), color, .2f);
        Debug.DrawLine(new(origin.x - offset, origin.y + offset), new(origin.x + offset, origin.y + offset), color, .2f);
        Debug.DrawLine(new(origin.x + offset, origin.y + offset), new(origin.x + offset, origin.y - offset), color, .2f);
        Debug.DrawLine(new(origin.x + offset, origin.y - offset), new(origin.x - offset, origin.y - offset), color, .2f);
    }
    
    public void ApplyColor(Color color)
    {
        defaultColor = color;
    }
    
    public void SetState(ITile.State newState) { }
    public void Select(int filter) { }
    public void Deselect() { }
}