using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GridSystem
{
    public interface IPiece: IHover
    {
        public string Name { get; set; }
        public int id { get; set; }
        public Coordinate coordinate { get; set; }
        public Action onClick { get; set; }
        public List<ITrait> traits { get; set; }

        public void Initialize()
        {
            RefreshId();
        }

        public void RefreshId()
        {
            var newId = (int)PieceType.generic;
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

        public T GetTrait<T>() where T : ITrait
        {
            T returnValue = default(T);
            traits ??= new();
            for (int i = 0; i < traits.Count; i++)
            {
                try
                {
                    returnValue = (T)traits[i];
                    break;
                }
                catch
                {
                    // ignored
                }
            }

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
        }
    }
}