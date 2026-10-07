using GridSystem;
using UnityEngine;

namespace GameData.Words.ConcreteWords
{
    public class Doll : Word
    {
        public override IPiece GetPiece()
        {
            IPiece piece = new PlayerInput.ObjectPiece(Object.Instantiate(Resources.Load<GameObject>("dummy")));
            pieces.Add(piece);
            return piece;
        }
    }
}