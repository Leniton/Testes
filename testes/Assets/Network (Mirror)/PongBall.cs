using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PongBall : NetworkBehaviour
{
    public float speed;
    Rigidbody2D rb;

    public PongNetworkManager networkManager;

    public override void OnStartServer()
    {
        base.OnStartServer();

        rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;

        Invoke("GameStart", 2);
    }

    void GameStart()
    {
        int side;
        do
        {
            side = Random.Range(-1, 2);

        } while (side == 0);

        rb.velocity = new Vector2(side * speed, 0);
    }

    [ServerCallback]
    void OnCollisionEnter2D(Collision2D c)
    {
        //print(c.transform.GetSiblingIndex());
        if(c.gameObject.tag == "Finish")
        {
            networkManager.playerList[c.transform.GetSiblingIndex()].pontuação++;

            rb.velocity = Vector2.zero;
            transform.position = Vector2.zero;
            Invoke("GameStart", 0.5f);
        }
        else if (c.gameObject.tag == "Player")
        {
            //print("foi");
            float distance = (c.transform.position.y - transform.position.y);
            //print(distance);

            if (distance > 0.3f && distance > 0)
            {
                //rb.AddForce(Vector2.down * 0.001f, ForceMode2D.Impulse);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y - 8, -10, 10));
            }
            else if (distance < -0.3f && distance < 0)
            {
                //rb.AddForce(Vector2.up * 0.001f, ForceMode2D.Impulse);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y + 8, -10, 10));
            }
        }
    }
}
