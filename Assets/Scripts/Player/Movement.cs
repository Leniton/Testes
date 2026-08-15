using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class Movement : MonoBehaviour
{
    public Vector2 input;
    bool move = false;

    private void Awake()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            transform.Translate(input);
            move = false;
            yield return Step();
        }
    }

    private IEnumerator Step()
    {
        float time = .3f;
        float step = time;
        while (step > 0)
        {
            if (step < time / 2f && move) yield break;
            yield return null;
            step -= Time.deltaTime;
        }
    }

    public void MoveNow()
    {
        move = true;
    }
}
