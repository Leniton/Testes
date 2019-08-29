using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LE_copy : MonoBehaviour
{

    Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        copiar(c);
    }

    public void copiar(Collision2D c)
    {
        if (c.gameObject.GetComponent<LE_copy>() != null || c.gameObject.tag == "Wall")
        {
            return;
        }else
        if (transform.childCount > 0)
        {
            Transform t = transform.GetChild(0);
            transform.DetachChildren();

            t.GetComponent<SpriteRenderer>().enabled = true;
            t.GetComponent<Collider2D>().enabled = true;
            t.GetComponent<LE_copy>().copiar(c);
            Destroy(gameObject);
            return;
        }
        GameObject copied = Instantiate(c.gameObject, transform.position, transform.rotation);
        copied.AddComponent<LE_copy>();
        GetComponent<Collider2D>().enabled = false;
        transform.SetParent(copied.transform);
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
