using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GridSystem.UI
{
    public class Piece : MonoBehaviour, IPiece
    {
        [SerializeField] private Image image;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int id { get; set; } = (int)PieceType.generic;

        private Coordinate _coordinate;

        public Coordinate coordinate
        {
            get => _coordinate;
            set => _coordinate = value;
        }

        public Action onEnter { get; set; }
        public Action onExit { get; set; }
        public Action onClick { get; set; }
        public List<Characteristic> characteristics { get; set; } = new();

        public void StylePiece(Sprite sprite, Color color)
        {
            image.sprite = sprite;
            image.color = color;
        }

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
        {
            if (newTile is not Tile tile) return;
            //take him out of previous place
            if (previousTile != null) previousTile.RemovePiece(this);
            //place him at new space
            transform.SetParent(tile.transform);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            coordinate = newCoordinates;
            newTile.PlacePiece(this);
        }
    }
}