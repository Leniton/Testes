using System;
using System.Collections.Generic;
using GameData.UI;
using GridSystem;
using UI.Utils;
using UI.Utils.Builder;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameData.Words
{
    public class Is : TextPiece
    {
        protected override void Awake()
        {
            base.Awake();
            ChangeText("is");
        }

        public override void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
        {
            base.SetCurrentTile(previousTile, newTile, newCoordinates);
            RemoveListeners(previousTile);
            GridManager.QueueTick(() =>
            {
                if (previousTile != null)
                    ModifySentences(IGrid.Instance.GetTileCoordinates(previousTile), BreakSentences);
                CheckSentence();
            });
            AddListeners(newTile);
        }

        private void RemoveListeners(ITile tile)
        {
            if (tile == null) return;
            var prevCoordinate = IGrid.Instance.GetTileCoordinates(tile);
            tile = IGrid.Instance.GetTileAt(prevCoordinate + Vector2.left);
            if (tile != null)
            {
                tile.onPiecePlaced -= PiecePlaced;
                tile.onPieceRemoved -= HorizontalWordRemoved;
            }
            tile = IGrid.Instance.GetTileAt(prevCoordinate + Vector2.right);
            if (tile != null)
            {
                tile.onPiecePlaced -= PiecePlaced;
                tile.onPieceRemoved -= HorizontalVerbRemoved;
            }
            tile = IGrid.Instance.GetTileAt(prevCoordinate + Vector2.up);
            if (tile != null)
            {
                tile.onPiecePlaced -= PiecePlaced;
                tile.onPieceRemoved -= VerticalWordRemoved;
            }
            tile = IGrid.Instance.GetTileAt(prevCoordinate + Vector2.down);
            if (tile != null)
            {
                tile.onPiecePlaced -= PiecePlaced;
                tile.onPieceRemoved -= VerticalVerbRemoved;
            }
        }

        private void AddListeners(ITile tile)
        {
            if (tile == null) return;
            var newCoordinate = IGrid.Instance.GetTileCoordinates(tile);
            tile = IGrid.Instance.GetTileAt(newCoordinate + Vector2.left);
            if (tile != null)
            {
                tile.onPiecePlaced += PiecePlaced;
                tile.onPieceRemoved += HorizontalWordRemoved;
            }
            tile = IGrid.Instance.GetTileAt(newCoordinate + Vector2.right);
            if (tile != null)
            {
                tile.onPiecePlaced += PiecePlaced;
                tile.onPieceRemoved += HorizontalVerbRemoved;
            }
            tile = IGrid.Instance.GetTileAt(newCoordinate + Vector2.up);
            if (tile != null)
            {
                tile.onPiecePlaced += PiecePlaced;
                tile.onPieceRemoved += VerticalWordRemoved;
            }
            tile = IGrid.Instance.GetTileAt(newCoordinate + Vector2.down);
            if (tile != null)
            {
                tile.onPiecePlaced += PiecePlaced;
                tile.onPieceRemoved += VerticalVerbRemoved;
            }
        }

        private void PiecePlaced(IPiece piece)
        {
            CheckSentence();
        }

        private void VerticalWordRemoved(IPiece piece) =>
            WordRemoved(piece, coordinate + Vector2.down);
        private void HorizontalWordRemoved(IPiece piece) =>
            WordRemoved(piece, coordinate + Vector2.right);
        private void WordRemoved(IPiece piece, Coordinate verbCoordinate)
        {
            GridManager.QueueTick(() =>
            {
                if (piece.GetTrait<ReferenceTrait<Word>>() is not { } wordRef) return;
                var word = wordRef.Value;
                var verbsRef = IGrid.Instance.GetTileAt(verbCoordinate).GetPiecesWith<ReferenceTrait<Verb>>();
                foreach (var verb in verbsRef)
                    verb.Value.Break(word);
            });
        }
        
        private void VerticalVerbRemoved(IPiece piece) =>
            VerbRemoved(piece, coordinate + Vector2.up);
        private void HorizontalVerbRemoved(IPiece piece) =>
            VerbRemoved(piece, coordinate + Vector2.left);
        private void VerbRemoved(IPiece piece, Coordinate verbCoordinate)
        {
            GridManager.QueueTick(() =>
            {
                if (piece.GetTrait<ReferenceTrait<Verb>>() is not { } verbRef) return;
                var verb = verbRef.Value;
                var wordsRef = IGrid.Instance.GetTileAt(verbCoordinate).GetPiecesWith<ReferenceTrait<Word>>();
                foreach (var word in wordsRef)
                    verb.Break(word.Value);
            });
        }

        private void CheckSentence() => ModifySentences(coordinate,MountSentences);
        private void ModifySentences(Coordinate center, 
            Action<List<ReferenceTrait<Word>>, List<ReferenceTrait<Verb>>> modifyMethod)
        {
            //horizontal
            var words = IGrid.Instance.GetTileAt(center + Vector2.left).GetPiecesWith<ReferenceTrait<Word>>();
            var verbs = IGrid.Instance.GetTileAt(center + Vector2.right).GetPiecesWith<ReferenceTrait<Verb>>();
            if (words is { Count: > 0 } && verbs is { Count: > 0 })
            {
                // Debug.Log("word found: horizontal");
                modifyMethod(words, verbs);
            }

            //vertical
            words = IGrid.Instance.GetTileAt(center + Vector2.up).GetPiecesWith<ReferenceTrait<Word>>();
            verbs = IGrid.Instance.GetTileAt(center + Vector2.down).GetPiecesWith<ReferenceTrait<Verb>>();
            if (words is { Count: > 0 } && verbs is { Count: > 0 })
            {
                // Debug.Log("word found: vertical");
                modifyMethod(words, verbs);
            }
        }

        private void MountSentences(List<ReferenceTrait<Word>> words, List<ReferenceTrait<Verb>> verbs)
        {
            for (int w = 0; w < words.Count; w++)
            {
                var word = words[w].Value;
                for (int v = 0; v < verbs.Count; v++)
                {
                    var verb = verbs[v].Value;
                    verb.Form(word);
                }
            }
        }
        private void BreakSentences(List<ReferenceTrait<Word>> words, List<ReferenceTrait<Verb>> verbs)
        {
            for (int w = 0; w < words.Count; w++)
            {
                var word = words[w].Value;
                for (int v = 0; v < verbs.Count; v++)
                {
                    var verb = verbs[v].Value;
                    verb.Break(word);
                }
            }
        }
    }
}