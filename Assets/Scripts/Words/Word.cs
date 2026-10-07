using System;
using System.Collections.Generic;
using GridSystem;

namespace GameData.Words
{
    public abstract class Word
    {
        protected List<IPiece> pieces = new();
        
        public abstract IPiece GetPiece();
        
        public void ForEachPiece(Action<IPiece> action)
        {
            foreach (var piece in pieces)
                action(piece);
        }
    }
}