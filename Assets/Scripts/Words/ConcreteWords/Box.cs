using GridSystem;
using UnityEngine;

namespace GameData.Words.ConcreteWords
{
    public class Box : Word
    {
        public override IPiece GetPiece()
        {
            IPiece piece = new PlayerInput.ObjectPiece(Object.Instantiate(Resources.Load<GameObject>("box")));
            piece.AddTrait(new GhostTrait());
            pieces.Add(piece);
            return piece;
        }
    }
}