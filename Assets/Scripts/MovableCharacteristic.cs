using System;
using GridSystem;
using UnityEngine;
using Util;
namespace GameData
{
    public class MovableCharacteristic : ICharacteristic
    {
        public IPiece Piece { get; private set; }

        public void SetUp(IPiece _piece) => Piece = _piece;

        public void TryMove(Vector2 direction, out Coordinate finalCoordinate, out ITile blockingTile)
        {
            finalCoordinate = Piece.coordinate;
            var coordinate = finalCoordinate;
            Coordinate step = new Coordinate(Math.Clamp((int)direction.x, -1, 1), Math.Clamp((int)direction.y, -1, 1));//update each step to afford uneven steps?
            Coordinate next = Piece.coordinate + step;
            Coordinate goal = Piece.coordinate + direction;
            // Debug.Log($"at {Piece.coordinate} => {direction} | {goal}");
            while (next != goal)
            {
                ITile targetTile = IGrid.Instance.GetTileAt(next);
                blockingTile = targetTile;
                if (!ValidTile(targetTile).OnFalse(() => Move(coordinate))) return;
                finalCoordinate = next;
                coordinate = finalCoordinate;
                next += step;
            }
            ITile goalTile = IGrid.Instance.GetTileAt(goal);
            blockingTile = goalTile;
            if (!ValidTile(goalTile).OnFalse(() => Move(coordinate))) return;
            finalCoordinate = goal;
            blockingTile = null;
            Move(finalCoordinate);
            return;

            bool ValidTile(ITile tile)
            {
                // Debug.Log($"checking {next}/{goal}");
                if (tile == null) return false;
                int id = tile.pieceID;
                return (id & 2) == 0;
            }
        }

        private void Move(Coordinate coordinate)
        {
            var currentTile = IGrid.Instance.GetTileAt(Piece.coordinate);
            var target = IGrid.Instance.GetTileAt(coordinate);
            if (target == null) return;
            Piece.SetCurrentTile(currentTile, target, coordinate);
        }
    }
}
