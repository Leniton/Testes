using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform_Movement : MonoBehaviour {

    enum Estado { parado, andando, pulando }

    [SerializeField] Estado estado = new Estado();

    [SerializeField] bool controleMovimento = true;
    [SerializeField] int vel;
    [SerializeField] int pulo;
    [SerializeField] bool puloCurto;
    [SerializeField] float fallSpeed;
    Rigidbody2D RB;

    [Space]
    [SerializeField]bool nochao;

    Collider2D chaoPisado;

    /*void OnDrawGizmos()
    {
        Vector2 size = GetComponent<BoxCollider2D>().size;
        float H = size.y * 0.5f;
        //Gizmos.DrawCube(transform.position + (Vector3.down * size.y * 0.8f), size - (Vector2.up * H));
        Gizmos.DrawWireCube(transform.position + (Vector3.down * size.y * 0.8f), size - (Vector2.up * H));
    }*/

    void Start()
    {
        estado = Estado.parado;
        RB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Mover();
        /*else if (RB.velocity.y == 0)
        {
            if (RB.velocity.x != 0)
            {
                estado = Estado.andando;
                //print("desceu andando");
            }
            else if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                estado = Estado.parado;
                //print("desceu");
            }
            nochao = true;
        }*/
    }

    void Mover()
    {
        if (controleMovimento)
        {

            if (Input.GetKeyDown(KeyCode.W))
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.pulando;
                        RB.AddForce(Vector2.up * pulo, ForceMode2D.Impulse);
                        nochao = false;
                        break;
                    case Estado.andando:
                        estado = Estado.pulando;
                        RB.AddForce(Vector2.up * pulo, ForceMode2D.Impulse);
                        nochao = false;
                        break;
                    case Estado.pulando:

                        break;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        RB.velocity = Vector3.left * vel;
                        break;
                    case Estado.andando:
                        if (RB.velocity.x > 0)
                        {
                            RB.velocity = Vector3.left * vel;
                        }
                        break;
                    case Estado.pulando:
                        RB.velocity = new Vector3(-1 * vel, RB.velocity.y, 0);
                        break;
                }
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //print(estado);
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        RB.velocity = Vector3.right * vel;
                        break;
                    case Estado.andando:
                        if (RB.velocity.x < 0)
                        {
                            RB.velocity = Vector3.right * vel;
                        }
                        break;
                    case Estado.pulando:
                        RB.velocity = new Vector3(1 * vel, RB.velocity.y, 0);
                        //print("no ar");
                        break;
                }
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                RB.velocity = new Vector2(0, RB.velocity.y);
                if (nochao)
                {
                    RB.velocity = Vector2.zero;
                    estado = Estado.parado;
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                switch (estado)
                {
                    case Estado.parado:

                        break;
                    case Estado.andando:

                        break;
                    case Estado.pulando:
                        /*controleMovimento = false;
                        RB.velocity = Vector2.zero;
                        RB.gravityScale = 0;
                        StartCoroutine("Queda");*/
                        break;
                }
            }
        }
        if (fallSpeed > 0 && (RB.velocity.y < 0 || (puloCurto && !Input.GetKey(KeyCode.W) && !nochao)))
        {
            //print("caindo");
            estado = Estado.pulando;
            Vector3 fall = RB.velocity;
            fall += Vector3.up * Physics2D.gravity.y * (RB.gravityScale + 10) * fallSpeed * Time.deltaTime;
            RB.velocity = fall;
        }
    }

    /*IEnumerator Queda()
    {
        RB.velocity = Vector2.up * 1;
        yield return new WaitForSeconds(0.5f);
        RB.gravityScale = 1;
        transform.localScale = new Vector3(0.8f, 1.2f, 1);

        Vector2 size = GetComponent<BoxCollider2D>().size;
        float H = size.y * 0.5f;

        do
        {
            Vector3 fall = RB.velocity;
            fall += Vector3.up * Physics2D.gravity.y * (RB.gravityScale + 20) * Time.deltaTime;
            RB.velocity = fall;
            yield return null;
            Debug.DrawRay(transform.position, Vector2.down, Color.red);
        } while (nochao);//Physics2D.OverlapBox(transform.position + (Vector3.down * size.y * (0.8f)), size - (Vector2.up * H), 0,0) != null);
        print("bateu");
        controleMovimento = true;
        transform.localScale = new Vector3(1, 1, 1);
    }*/

    void OnCollisionEnter2D(Collision2D c)
    {
        //print("normal:"+c.GetContact(0).normal);
        if(c.collider.tag == "Chão")
        {
            if (c.GetContact(0).normal.y >= 0.4f)
            {
                RB.velocity = new Vector2(RB.velocity.x, 0);
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                {
                    estado = Estado.andando;
                    //print("desceu andando");
                }
                else
                {
                    estado = Estado.parado;
                    //print("desceu");
                }
                nochao = true;
                chaoPisado = c.collider;
            }
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        if (c.collider == chaoPisado)
        {
            nochao = false;
        }
    }
}
