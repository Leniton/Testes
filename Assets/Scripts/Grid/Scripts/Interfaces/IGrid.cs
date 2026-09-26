using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public interface IGrid
    {
        public static IGrid Instance;

        public int Width { get; }
        public int Height { get; }

        public List<ITile> tiles { get; set; }
        public ITile hoveredTile { get; set; }

        public Action<ITile> onClick { get; set; }
        public Action<ITile> onEnter { get; set; }
        public Action<ITile> onExit { get; set; }
        
        public SelectData selectData { get; set; }

        public void SetUpGrid();

        public int GetTileIndex(ITile tile) => tiles.IndexOf(tile);

        public Coordinate GetTileCoordinates(ITile tile)
        {
            int id = GetTileIndex(tile);
            int x = id % Width;
            int y = id / Width;

            //print($"x:{x} | y:{y}");
            return new Coordinate(x, y);
        }

        public ITile GetTileAt(Coordinate coordinates) =>
            (coordinates.x < 0 || coordinates.y < 0 || coordinates.x >= Width || coordinates.y >= Height)
                ? null
                : tiles[(Width * coordinates.y) + coordinates.x];

        public void WarpToSpot(IPiece piece, Coordinate coordinates) =>
            IPiece.PlacePieceOnTile(piece, GetTileAt(coordinates), coordinates, GetTileAt(piece.coordinate));

        public void ChooseTileInRange(Coordinate origin, Area area, Area selectArea,
            Action<SelectTileEvent> onSelectTile,
            int filter = -1, int selectFilter = -1)
        {
            selectData = new()
            {
                origin = origin,
                selectArea = selectArea,
                selectFilter = selectFilter,
                area = area,
                filter = filter,
                onSelectTile = onSelectTile,
            };

            List<Coordinate> coordinates = area.GetCoordinates(origin, Width, Height);
            for (int i = 0; i < coordinates.Count; i++)
            {
                Coordinate currentCoordinate = coordinates[i];
                //Debug.Log($"{currentCoordinate.x} | {currentCoordinate.y}");
                ITile tile = GetTileAt(currentCoordinate);
                if (tile == null) continue;
                tile.onSelectionEnter += SelectArea;
                tile.onSelectionExit += UnSelectArea;
                tile.onPickTile += ClickArea;
                tile.SetState(IsInFilter(tile.pieceID, filter) ? ITile.State.selectable : ITile.State.invalid);
                // if (IsInFilter(tile.pieceID, filter))
                // {
                //     tile.state = ITile.State.selectable;
                //     tile.AddColor(tile.selectableColor);
                // }
                // else
                // {
                //     tile.state = ITile.State.invalid;
                //     tile.AddColor(tile.invalidColor);
                // }
            }

            if (hoveredTile == null || !coordinates.Contains(GetTileCoordinates(hoveredTile))) return;

            SelectArea(hoveredTile);
        }

        public void SelectArea(ITile tile)
        {
            //return;
            if (selectData == null) return;
            Coordinate origin = GetTileCoordinates(tile);
            Area area = selectData.selectArea;
            int filter = selectData.selectFilter;

            List<Coordinate> coordinates = area.GetCoordinates(origin, Width, Height);
            for (int i = 0; i < coordinates.Count; i++)
            {
                ITile currentTile = GetTileAt(coordinates[i]);
                currentTile?.Select(filter);
                // if (currentTile != null)
                // {
                //     currentTile.AddColor(IsInFilter(currentTile.pieceID, filter)
                //         ? currentTile.validColor
                //         : currentTile.invalidColor);
                // }
            }
        }

        public void UnSelectArea(ITile tile)
        {
            //return;
            if (selectData == null) return;
            Coordinate origin = GetTileCoordinates(tile);
            Area area = selectData.selectArea;

            List<Coordinate> coordinates = area.GetCoordinates(origin, Width, Height);
            for (int i = 0; i < coordinates.Count; i++)
            {
                ITile currentTile = GetTileAt(coordinates[i]);
                currentTile?.Deselect();
                // if (currentTile != null)
                // {
                //     var color = IsInFilter(currentTile.pieceID, filter) ? currentTile.validColor : currentTile.invalidColor;
                //     if (currentTile.state == ITile.State.generic) currentTile.SetColor(currentTile.defaultColor);
                //     currentTile.RemoveColor(color);
                // }
            }
        }

        public void ClickArea(ITile tile)
        {
            //return;
            if (selectData == null) return;
            Coordinate point = GetTileCoordinates(tile);
            Coordinate origin = selectData.origin;
            Area area = selectData.selectArea;
            int filter = selectData.selectFilter;
            var clickAction = selectData.onSelectTile;

            List<Coordinate> coordinates = area.GetCoordinates(point, Width, Height);
            for (int i = 0; i < coordinates.Count; i++)
            {
                ITile currentTile = GetTileAt(coordinates[i]);
                if (currentTile != null && IsInFilter(currentTile.pieceID, filter))
                {
                    clickAction?.Invoke(new()
                    {
                        origin = origin, 
                        point = point, 
                        area = area,
                    });
                    return;
                }
            }
        }

        public void StopSelecting()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].SetState(ITile.State.generic);
                tiles[i].Deselect();
                tiles[i].onSelectionEnter -= SelectArea;
                tiles[i].onSelectionExit -= UnSelectArea;
                tiles[i].onPickTile -= ClickArea;
            }

            selectData = null;
        }

        public void OnClick(ITile tile)
        {
            onClick?.Invoke(tile);
        }

        public void OnEnter(ITile tile)
        {
            hoveredTile = tile;
            onEnter?.Invoke(tile);
        }

        public void OnExit(ITile tile)
        {
            hoveredTile = null;
            onExit?.Invoke(tile);
        }

        public static bool IsInFilter(int value, int filter) =>
            filter <= 0 || NumberUtil.ContainsAnyBits(value, filter);
    }

    public class SelectData
    {
        public Coordinate origin;
        public Area selectArea;
        public int selectFilter;
        public Area area;
        public int filter;

        //not sure if needed
        public Action<SelectTileEvent> onSelectTile;
    }

    public class SelectTileEvent
    {
        public Coordinate origin;
        public Coordinate point;
        public Area area;
    }
}