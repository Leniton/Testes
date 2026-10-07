using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GridSystem
{
    public interface IPiece: IHover
    {
        public const int GENERIC = 2;
        
        public string Name { get; set; }
        public int id { get; set; }
        public Coordinate coordinate { get; set; }
        public Action onClick { get; set; }
        public List<ITrait> traits { get; set; }
        public Action<ITile, ITile> onTileChanged { get; set; }

        public void Initialize()
        {
            RefreshId();
        }

        public void RefreshId()
        {
            var newId = GENERIC;
            traits ??= new();
            for (int i = 0; i < traits.Count; i++)
                traits[i].ModifyID(ref newId);
            id = newId;
        }

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates);

        public bool AddTrait<T>(T trait) where T : ITrait
        {
            traits ??= new();
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].GetType().IsAssignableFrom(typeof(T)) ||
                    traits[i].GetType().IsSubclassOf(typeof(T)))
                {
                    Debug.LogWarning($"Trait {traits[i].GetType().Name} already exists!");
                    return false;
                }
            }

            //Debug.Log($"adding {trait.GetType().Name}");
            traits.Add(trait);
            trait.SetUp(this);
            RefreshId();
            return true;
        }

        public bool RemoveTrait<T>(T trait) where T : ITrait
        {
            bool removed = traits.Remove(trait);
            if (removed) RefreshId();
            return removed;
        }

        public bool RemoveTrait<T>() where T : ITrait
        {
            traits ??= new();
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i] is not T) continue;
                traits.RemoveAt(i);
                RefreshId();
                return true;
            }
            return false;
        }

        public T GetTrait<T>() where T : ITrait
        {
            T returnValue = default(T);
            traits ??= new();
            for (int i = 0; i < traits.Count; i++)
                if (traits[i] is T trait) 
                    return trait;

            return returnValue;
        }

        public string TraitsInfo()
        {
            traits ??= new();
            StringBuilder value = new();

            for (int i = 0; i < traits.Count; i++)
            {
                value.Append(traits[i]);
                if (i < traits.Count - 1) value.Append('\n');
            }

            return value.ToString();
        }

        public string ToString()
        {
            string value = Name;
            value += $"({coordinate.x},{coordinate.y})";
            value += TraitsInfo();

            return value;
        }

        public static void PlacePieceOnTile(IPiece piece, ITile tile, Coordinate coordinates, ITile currentTile = null)
        {
            if (piece == null) return;
            piece.coordinate = coordinates;
            currentTile?.RemovePiece(piece);
            tile?.PlacePiece(piece);
            piece.SetCurrentTile(currentTile, tile, coordinates);
            piece.onTileChanged?.Invoke(currentTile, tile);
        }
    }
}