using System;
using System.Collections;
using GameData;
using GridSystem;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class Movement : MonoBehaviour
{
    public Vector2 input;
    bool move = false;
    bool reset = false;

    public IPiece piece;

    private void MovePiece()
    {
        if (input == Vector2.zero) return;
        var current = IGrid.Instance.GetTileAt((Vector2)transform.position);
        var movable = piece.GetTrait<MovableTrait>();
        if (movable != null)
        {
            movable.TryMove(input);
            return;
        }
        Coordinate coordinate = (Vector2)transform.position + input;
        var target = IGrid.Instance.GetTileAt(coordinate);
        if (target == null) return;
        var targetId = target.pieceID;
        if ((targetId & 1) == 0) return;//not empty
        IPiece.PlacePieceOnTile(piece, target, coordinate, current);
    }

    public void MoveNow(Vector2 direction)
    {
        input = direction;
        MovePiece();
    }

    public void ResetMovement()
    {
        if (move) reset = true;
        else input = Vector2.zero;
    }
}
