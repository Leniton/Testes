using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LE_move : MonoBehaviour
{

    Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        StartCoroutine("Move");
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        RB.velocity = Vector2.zero;
        RB.angularVelocity = 0;

        float x = Random.Range(-3, 4);
        float y = Random.Range(-3, 4);

        RB.AddForce(new Vector2(x, y), ForceMode2D.Impulse);

        StartCoroutine("Move");
    }
}
