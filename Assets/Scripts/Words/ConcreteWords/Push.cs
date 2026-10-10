using UnityEngine;

namespace GameData.Words.ConcreteWords
{
    public class Push : Verb
    {
        public override void Form(Word word)
        {
            if (words.Contains(word)) return;
            words.Add(word);
            word.ForEachPiece(piece =>
            {
                piece.RemoveTrait<GhostTrait>();
                var movable = piece.GetTrait<MovableTrait>();
                if (movable != null) return;
                piece.AddTrait(new MovableTrait());
            });
        }

        public override void Break(Word word)
        {
            if (!words.Contains(word)) return;
            words.Remove(word);
            word.ForEachPiece(piece =>
            {
                piece.AddTrait(new GhostTrait());
                var movable = piece.GetTrait<MovableTrait>();
                if (movable != null)
                    piece.RemoveTrait<MovableTrait>();
            });
        }
    }
}