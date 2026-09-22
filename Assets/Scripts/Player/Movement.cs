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

    private void Awake()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            move = false;
            MovePiece();
            if (reset)
            {
                input = Vector2.zero;
                reset = false;
            }
            yield return Step();
        }
    }

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

    private IEnumerator Step()
    {
        if (input == Vector2.zero) yield break;
        float time = .3f;
        float step = time;
        while (step > 0)
        {
            if (step < time / 2f && move) yield break;
            yield return null;
            step -= Time.deltaTime;
        }
    }

    public void MoveNow(Vector2 direction)
    {
        input = direction;
        move = true;
    }

    public void ResetMovement()
    {
        if (move) reset = true;
        else input = Vector2.zero;
    }
}
