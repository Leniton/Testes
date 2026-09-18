using System;
using GridSystem;
using UnityEngine;
namespace GameData
{
    public class MovableCharacteristic : ICharacteristic
    {
        public IPiece Piece { get; private set; }

        public void SetUp(IPiece _piece) => Piece = _piece;

        public void TryMove(Vector2 direction, out Coordinate finalCoordinate)
        {
            finalCoordinate = Piece.coordinate;
            Coordinate step = new Coordinate(Math.Clamp((int)direction.x, -1, 1), Math.Clamp((int)direction.y, -1, 1));//update each step to afford uneven steps?
            Coordinate next = Piece.coordinate + step;
            Coordinate goal = Piece.coordinate + direction;
            // Debug.Log($"at {Piece.coordinate} => {direction} | {goal}");
            while (next != goal)
            {
                ITile targetTile = IGrid.Instance.GetTileAt(next);
                if (!ValidTile(targetTile)) return;
                finalCoordinate = next;
                next += step;
            }
            ITile goalTile = IGrid.Instance.GetTileAt(goal);
            if (ValidTile(goalTile)) 
                finalCoordinate = goal;
            return;

            bool ValidTile(ITile tile)
            {
                // Debug.Log($"checking {next}/{goal}");
                if (tile == null) return false;
                int id = tile.pieceID;
                return (id & 2) == 0;
            }
        }
    }
}
