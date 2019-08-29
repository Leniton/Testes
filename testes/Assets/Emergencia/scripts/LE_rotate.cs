using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LE_rotate : MonoBehaviour
{

    Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        StartCoroutine("Rotate");
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(1);

        RB.angularVelocity = 0;

        yield return new WaitForSeconds(Random.Range(1, 3));
        
        RB.AddTorque((float)Random.Range(-0.5f,0.5f),ForceMode2D.Impulse);
        StartCoroutine("Rotate");
    }

}
