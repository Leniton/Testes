using System.Collections.Generic;
using GridSystem;
using InputSystemHelper;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameData.Words.ConcreteWords
{
    public class You : Verb
    {
        private List<Word> words = new();

        private Vector2 input;

        public You()
        {
            var move = InputSystemHelper.Input.Map("Player").Action("Move");
            move.performed += OnInput;
            move.canceled += OnMoveCanceled;
        }

        public override void Form(Word word)
        {
            words.Add(word);
            word.ForEachPiece(piece =>
            {
                piece.AddTrait(new YouTrait());
                var movable = piece.GetTrait<MovableTrait>();
                if (movable != null) return;
                piece.AddTrait(new MovableTrait(0));
            });
        }

        public override void Break(Word word)
        {
            words.Remove(word);
            word.ForEachPiece(piece =>
            {
                piece.RemoveTrait<YouTrait>();
                var movable = piece.GetTrait<MovableTrait>();
                if (movable != null)
                    piece.RemoveTrait<MovableTrait>();
            });
        }

        private void OnInput(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            List<IPiece> movedPieces = new();
            for (int i = 0; i < words.Count; i++)
                words[i].ForEachPiece(Move);
            return;

            void Move(IPiece piece)
            {
                if (movedPieces.Contains(piece)) return;
                movedPieces.Add(piece);
                MovePiece(piece, direction);
            }
        }

        private void OnMoveCanceled(InputAction.CallbackContext obj)
        {
            input = Vector2.zero;
        }

        private void MovePiece(IPiece piece, Vector2 direction)
        {
            if (direction == Vector2.zero) return;
            var movable = piece.GetTrait<MovableTrait>();
            if (movable == null) return;
            movable.TryMove(direction);
        }
        
        public class YouTrait : ITrait
        {
            public int idModifier { get; } = ITile.EMPTY;
            public IPiece Piece { get; private set; }
            public void SetUp(IPiece _piece)
            {
                Piece = _piece;
            }
        }
    }
}