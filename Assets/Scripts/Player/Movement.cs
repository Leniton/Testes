using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class Movement : MonoBehaviour
{
    public Vector2 input;
    bool move = false;
    bool reset = false;

    private void Awake()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            move = false;
            GridManager.SetElement(transform.position, null);
            transform.Translate(input);
            GridManager.SetElement(transform.position, gameObject);
            if (reset)
            {
                input = Vector2.zero;
                reset = false;
            }
            yield return Step();
        }
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
