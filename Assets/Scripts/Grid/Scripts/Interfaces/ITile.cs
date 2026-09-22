using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public interface ITile
    {
        public List<IPiece> pieces { get; set; }

        public int pieceID
        {
            get
            {
                if (pieces.Count <= 0) return 1;
                int value = 2;
                for (int i = 0; i < pieces.Count; i++)
                    value |= pieces[i].id;

                return value;
            }
        }

        public enum State
        {
            generic,
            selectable,
            invalid
        }

        public State state { get; }

        //callbacks
        public Action<ITile> onClick { get; set; }
        public Action<ITile> onEnter { get; set; }
        public Action<ITile> onExit { get; set; }

        public void PlacePiece(IPiece piece)
        {
            if (!pieces.Contains(piece))
                pieces.Add(piece);
        }

        public void RemovePiece(IPiece piece) => pieces.Remove(piece);

        public IPiece GetFirstPiece() => pieces[0];

        public IPiece GetPiece(int id = 0) => id < pieces.Count ? pieces[id] : null;

        public T GetFirstWith<T>() where T : ITrait
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                IPiece piece = pieces[i];
                T c = piece.GetCharacteristic<T>();
                if (c != null) return c;
            }

            return default(T);
        }

        public T GetLastWith<T>() where T : ITrait
        {
            for (int i = pieces.Count - 1; i >= 0; i--)
            {
                IPiece piece = pieces[i];
                T c = piece.GetCharacteristic<T>();
                if (c != null) return c;
            }

            return default(T);
        }

        public List<T> GetPiecesWith<T>() where T : ITrait
        {
            List<T> returnValue = new();
            for (int i = 0; i < pieces.Count; i++)
            {
                IPiece piece = pieces[i];
                T c = piece.GetCharacteristic<T>();
                if (c != null) returnValue.Add(c);
            }

            return returnValue;
        }

        public void SetState(State newState);

        public void Select(int filter);
        public void Deselect();
    }
}
