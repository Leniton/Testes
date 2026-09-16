using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GridSystem.VisualElements
{
    public class Piece : Image, IPiece
    {
        public string Name { get; set; }
        public int id { get; set; } = (int)PieceType.generic;

        public Coordinate coordinate { get; set; }
        public Action onEnter { get; set; }
        public Action onExit { get; set; }
        public Action onClick { get; set; }
        public List<Characteristic> characteristics { get; set; } = new();

        public Piece()
        {
            style.position = Position.Absolute;
            style.flexGrow = 1;
            transform.scale = Vector2.one * 2;
        }

        public void StylePiece(Sprite sprite, Color color)
        {
            this.sprite = sprite;
            tintColor = color;
        }

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
        {
            if (newTile is not Tile) return;
            //take him out of previous place
            if (previousTile != null) previousTile.RemovePiece(this);

            Tile tile = (Tile)newTile;
            tile.Add(this);
            transform.position = Vector3.zero;
            coordinate = newCoordinates;
            newTile.PlacePiece(this);
        }
    }
}