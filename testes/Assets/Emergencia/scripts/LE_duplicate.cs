using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LE_duplicate : MonoBehaviour
{

    Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.GetComponent<LE_duplicate>() != null || c.gameObject.tag == "Wall")
        {
            return;
        }

        GameObject copy = Instantiate(gameObject);
        RB.AddForce(transform.right * 5, ForceMode2D.Impulse);
        copy.GetComponent<Rigidbody2D>().AddForce(-transform.right * 5, ForceMode2D.Impulse);
        StartCoroutine("TimeToDie");
    }

    IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(3);
        print("dead");
        if (gameObject.activeSelf)
        {
            Destroy(gameObject);
        }
    }
}
