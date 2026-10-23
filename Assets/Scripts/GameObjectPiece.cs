using System;
using System.Collections.Generic;
using GridSystem;
using UnityEngine;

namespace GameData
{
    public class GameObjectPiece : IPiece
    {
        public Action onEnter { get; set; }
        public Action onExit { get; set; }
        public string Name { get; set; }
        public int id { get; set; }
        public Coordinate coordinate { get; set; }
        public Action onClick { get; set; }
        public List<ITrait> traits { get; set; } = new();
        public Action<ITile, ITile> onTileChanged { get; set; }
        
        private GameObject target;
        
        public GameObjectPiece(GameObject target, Action<ITile,ITile> OnTileChanged = null)
        {
            this.target = target;
            coordinate = target.transform.localPosition;
            var piece = this as IPiece;
            onTileChanged += OnTileChanged;
            piece?.Initialize();
        }

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
        {
            previousTile?.RemovePiece(this);
            newTile?.PlacePiece(this);
            coordinate = newCoordinates;
            target.transform.localPosition = newCoordinates;
        }
    }
}