using GridSystem;
using UnityEngine;

namespace GameData.Words.ConcreteWords
{
    public class Text : Word
    {
        private TextPiece piecePrefab;
        private Is isPrefab;

        public override IPiece GetPiece() => GetText("te\nxt", Color.mediumPurple);

        public TextPiece GetText(string text, Color? textColor = null, Color? backgroundColor = null)
        {
            piecePrefab ??= Resources.Load<TextPiece>("text");
            var piece = Object.Instantiate(piecePrefab);
            piece.ChangeText(text, textColor, backgroundColor);
            pieces.Add(piece);
            return piece;
        }
        public Is GetIs()
        {
            isPrefab ??= Resources.Load<Is>("is");
            var piece = Object.Instantiate(isPrefab);
            pieces.Add(piece);
            return piece;
        }
    }
}