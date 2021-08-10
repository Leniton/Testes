using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PongPlayer : NetworkBehaviour
{
    public int pontuação = 0;
    public Text texto;

    public float speed;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (SystemInfo.supportsAccelerometer)
            {
                rb.velocity = new Vector2(0, Input.acceleration.y * speed);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.velocity = Vector2.up * speed;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.velocity = Vector2.down * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        texto.text = pontuação.ToString(); ;
    }
}
