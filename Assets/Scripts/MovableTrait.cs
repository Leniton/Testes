using System;
using GridSystem;
using UnityEngine;
using Util;
namespace GameData
{
    public class MovableTrait : ITrait
    {
        public IPiece Piece { get; private set; }

        public int pushLimit { get; private set; }

        private bool moving;
        
        public MovableTrait(int _pushLimit = -1) => pushLimit = _pushLimit;

        public void SetUp(IPiece _piece)
        {
            if (Piece != null) Piece.onTileChanged -= OnPieceMoved;
            Piece = _piece;
            if (Piece != null) Piece.onTileChanged += OnPieceMoved;
        }

        private void OnPieceMoved(ITile currentTile, ITile newTile)
        {
            moving = false;
        }
        
        public (bool moved, Coordinate finalPosition) TryMove(Vector2 direction, int? maxPush = null)
        {
            var finalCoordinate = Piece.coordinate;
            if (moving) return (false, finalCoordinate);
            moving = true;
            int limit = maxPush ?? pushLimit;
            bool push = limit != 0;
            Coordinate step = new Coordinate(Math.Clamp((int)direction.x, -1, 1), Math.Clamp((int)direction.y, -1, 1));//update each step to afford uneven steps?
            Coordinate next = Piece.coordinate + step;
            Coordinate goal = Piece.coordinate + direction;
            // Debug.Log($"at {Piece.coordinate} => {direction} | {goal}");
            while (next != goal)
            {
                ITile targetTile = IGrid.Instance.GetTileAt(next);
                if (!ValidTile(targetTile).OnFalse(() => PushAndMove(finalCoordinate, targetTile)))
                    return (Moved(), finalCoordinate);
                finalCoordinate = next;
                next += step;
            }
            ITile goalTile = IGrid.Instance.GetTileAt(goal);
            if (!ValidTile(goalTile).OnFalse(() => PushAndMove(finalCoordinate, goalTile)))
                return (Moved(), finalCoordinate);
            finalCoordinate = goal;
            PushAndMove(finalCoordinate, goalTile);
            return (Moved(), finalCoordinate);

            bool ValidTile(ITile tile)
            {
                // Debug.Log($"checking {next}/{goal}");
                bool valid = tile != null;
                if (valid) valid = (tile.pieceID & ITile.EMPTY) != 0;
                if (!valid) valid = TryPush(tile, direction, limit);
                return valid;
            }

            bool Moved() => finalCoordinate != Piece.coordinate;

            void PushAndMove(Coordinate coordinate, ITile targetTile)
            {
                if (push) TryPush(targetTile, direction, limit);
                MovePiece(coordinate);
            }
        }

        private void MovePiece(Coordinate coordinate)
        {
            if (coordinate == Piece.coordinate) return;
            var currentTile = IGrid.Instance.GetTileAt(Piece.coordinate);
            var target = IGrid.Instance.GetTileAt(coordinate);
            if (target == null) return;
            IPiece.PlacePieceOnTile(Piece, target, coordinate, currentTile);
        }

        private bool TryPush(ITile tile, Vector2 direction, int maxPush)
        {
            if (tile == null) return false;
            var pieces = tile.GetPiecesWith<MovableTrait>();
            pieces ??= new();
            int id = 0;
            while (id < pieces.Count)
            {
                var movable = pieces[id];
                int push = movable?.pushLimit ?? 0;
                if (movable == null || movable.pushLimit == 0 || !movable.TryMove(direction, --push).moved) id++;
            }
            return tile.pieces is { Count: <= 0 };
        }
    }
}
