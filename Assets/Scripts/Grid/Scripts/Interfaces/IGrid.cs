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
        public bool currentlySelecting { get; set; }

        public Action<ITile> onClick { get; set; }
        public Action<ITile> onEnter { get; set; }
        public Action<ITile> onExit { get; set; }

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
            piece.SetCurrentTile(GetTileAt(piece.coordinate), GetTileAt(coordinates), coordinates);

        public void ChooseTileInRange(Coordinate origin, Area area, Area selectArea,
            Action<Coordinate, Coordinate, Area> onSelectTile,
            int filter = -1, int selectFilter = -1)
        {
            currentlySelecting = true;

            List<Coordinate> coordinates = area.GetCoordinates(origin, Width, Height);
            for (int i = 0; i < coordinates.Count; i++)
            {
                Coordinate currentCoordinate = coordinates[i];
                //Debug.Log($"{currentCoordinate.x} | {currentCoordinate.y}");
                ITile tile = GetTileAt(currentCoordinate);
                if (tile != null)
                {
                    tile.onEnter += (value) => SelectArea(currentCoordinate, selectArea, selectFilter);
                    tile.onExit += (value) => UnSelectArea(currentCoordinate, selectArea, selectFilter);
                    tile.onClick += (value) =>
                        ClickArea(onSelectTile, origin, currentCoordinate, selectArea, selectFilter);
                    if (IsInFilter(tile.pieceID, filter))
                    {
                        tile.state = (ITile.State.selectable);
                        tile.AddColor(tile.selectableColor);
                    }
                    else
                    {
                        tile.state = (ITile.State.invalid);
                        tile.AddColor(tile.invalidColor);
                    }
                }
            }

            if (hoveredTile == null || !coordinates.Contains(GetTileCoordinates(hoveredTile))) return;

            SelectArea(GetTileCoordinates(hoveredTile), selectArea, selectFilter);
        }

        public void SelectArea(Coordinate origin, Area area, int filter = -1)
        {
            //return;
            if (!currentlySelecting) return;

            List<Coordinate> coordinates = area.GetCoordinates(origin, Width, Height);
            for (int i = 0; i < coordinates.Count; i++)
            {
                ITile currentTile = GetTileAt(coordinates[i]);
                if (currentTile != null)
                {
                    currentTile.AddColor(IsInFilter(currentTile.pieceID, filter)
                        ? currentTile.validColor
                        : currentTile.invalidColor);
                }
            }
        }

        public void UnSelectArea(Coordinate origin, Area area, int filter = -1)
        {
            //return;
            if (!currentlySelecting) return;

            List<Coordinate> coordinates = area.GetCoordinates(origin, Width, Height);
            for (int i = 0; i < coordinates.Count; i++)
            {
                ITile currentTile = GetTileAt(coordinates[i]);
                Color color = Color.white;
                if (currentTile != null)
                {
                    color = IsInFilter(currentTile.pieceID, filter) ? currentTile.validColor : currentTile.invalidColor;
                    if (currentTile.state == ITile.State.generic) currentTile.SetColor(currentTile.defaultColor);
                    currentTile.RemoveColor(color);
                }
            }
        }

        public void ClickArea(Action<Coordinate, Coordinate, Area> clickAction, Coordinate origin, Coordinate point,
            Area area, int filter = -1)
        {
            //return;
            if (!currentlySelecting) return;

            List<Coordinate> coordinates = area.GetCoordinates(point, Width, Height);
            for (int i = 0; i < coordinates.Count; i++)
            {
                ITile currentTile = GetTileAt(coordinates[i]);
                Color color = Color.white;
                if (currentTile != null && IsInFilter(currentTile.pieceID, filter))
                {
                    clickAction?.Invoke(origin, point, area);
                    return;
                }
            }
        }

        public void StopSelecting()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].state = ITile.State.generic;
                tiles[i].SetColor(tiles[i].defaultColor);
                tiles[i].onClick = OnClick;
                tiles[i].onEnter = OnEnter;
                tiles[i].onExit = OnExit;
            }

            currentlySelecting = false;
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
}