using System;
using GridSystem;
using UnityEngine;
using Util;
namespace GameData
{
    public class MovableTrait : ITrait
    {
        public IPiece Piece { get; private set; }

        public void SetUp(IPiece _piece) => Piece = _piece;

        public bool TryMove(Vector2 direction)
        {
            var finalCoordinate = Piece.coordinate;
            Coordinate step = new Coordinate(Math.Clamp((int)direction.x, -1, 1), Math.Clamp((int)direction.y, -1, 1));//update each step to afford uneven steps?
            Coordinate next = Piece.coordinate + step;
            Coordinate goal = Piece.coordinate + direction;
            // Debug.Log($"at {Piece.coordinate} => {direction} | {goal}");
            while (next != goal)
            {
                ITile targetTile = IGrid.Instance.GetTileAt(next);
                if (!ValidTile(targetTile).OnFalse(() => MovePiece(finalCoordinate))) return Moved();
                finalCoordinate = next;
                next += step;
            }
            ITile goalTile = IGrid.Instance.GetTileAt(goal);
            if (!ValidTile(goalTile).OnFalse(() => MovePiece(finalCoordinate))) return Moved();
            finalCoordinate = goal;
            MovePiece(finalCoordinate);
            return Moved();

            bool ValidTile(ITile tile)
            {
                // Debug.Log($"checking {next}/{goal}");
                bool valid = tile != null;
                if (valid) valid = (tile.pieceID & 1) != 0;
                if (!valid) valid = TryPush(tile, direction);
                return valid;
            }

            bool Moved() => finalCoordinate != Piece.coordinate;
        }

        private void MovePiece(Coordinate coordinate)
        {
            var currentTile = IGrid.Instance.GetTileAt(Piece.coordinate);
            var target = IGrid.Instance.GetTileAt(coordinate);
            if (target == null) return;
            IPiece.PlacePieceOnTile(Piece, target, coordinate, currentTile);
        }

        private bool TryPush(ITile tile, Vector2 direction)
        {
            if (tile == null) return false;
            var pieces = tile.pieces;
            pieces ??= new();
            int id = 0;
            while (id < pieces.Count)
            {
                var movable = pieces[id]?.GetTrait<MovableTrait>();
                if (movable == null || !movable.TryMove(direction)) id++;
            }
            return tile.pieces is { Count: <= 0 };
        }
    }
}
