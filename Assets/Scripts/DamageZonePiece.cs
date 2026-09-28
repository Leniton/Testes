using System;
using System.Collections.Generic;
using GridSystem;
using UI.Utils.IValue;
using UnityEngine;

namespace GameData
{
    public class DamageZonePiece : MonoBehaviour, IPiece
    {
        public Action onEnter { get; set; }
        public Action onExit { get; set; }
        public string Name { get; set; }
        public int id { get; set; }
        public Coordinate coordinate { get; set; }
        public Action onClick { get; set; }
        public List<ITrait> traits { get; set; }
        public Action<ITile, ITile> onTileChanged { get; set; }

        private void Awake()
        {
            var piece = this as IPiece;
            piece.AddTrait(new GhostTrait());
        }

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
        {
            RemoveListeners(previousTile);
            AddListeners(newTile);
            transform.localPosition = newCoordinates;
        }

        private void AddListeners(ITile tile)
        {
            if (tile == null) return;
            tile.onPiecePlaced += OnPiecePlaced;
        }

        private void RemoveListeners(ITile tile)
        {
            if (tile == null) return;
            tile.onPiecePlaced -= OnPiecePlaced;
        }

        private void OnPiecePlaced(IPiece piece)
        {
            var health = piece.GetTrait<HealthTrait>();
            if (health == null) return;
            health.Damage(new Value<int>(1));
        }
        
        private class GhostTrait : ITrait
        {
            public IPiece Piece { get; private set; }
            public int idModifier => 1;

            public void SetUp(IPiece _piece)
            {
                Piece = _piece;
            }
        }
    }
}