using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform_Movement : MonoBehaviour {

    public enum Estado { parado, andando, pulando }

    //[HideInInspector] 
	public bool left, right, jump;

    [SerializeField] public Estado estado = new Estado();

    [SerializeField] bool controleMovimento = true;
    [SerializeField] int vel;
    [SerializeField] int pulo;
    [SerializeField] bool puloCurto;
    [SerializeField] float fallSpeed;
    Rigidbody2D RB;

    
    public bool nochao { get; private set; }

    public Collider2D chaoPisado { get; private set; }

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

    void FixedUpdate()
    {
        Mover();
    }

    void Mover()
    {
        if (controleMovimento)
        {

            if (jump)
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
                jump = false;
            }
            if (left)
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        RB.velocity = Vector3.left * vel;
                        break;
                    case Estado.andando:
                        //if (RB.velocity.x > 0)
                        //{
                            RB.velocity = Vector3.left * vel;
                        //}

                        if (!nochao) 
                        {
                            estado = Estado.pulando;
                        }
                        break;
                    case Estado.pulando:
                        RB.velocity = new Vector3(-1 * vel, RB.velocity.y, 0);
                        break;
                }
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (right)
            {
                //print(estado);
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        RB.velocity = Vector3.right * vel;
                        break;
                    case Estado.andando:
                        //if (RB.velocity.x < 0)
                        //{
                            RB.velocity = Vector3.right * vel;
                        //}

                        if (!nochao)
                        {
                            estado = Estado.pulando;
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
                else
                {
                    estado = Estado.pulando;
                }
            }
        }
        if (fallSpeed > 0 && (RB.velocity.y < 0 || (puloCurto && !jump && !nochao)))
        {
            //print("caindo");
            estado = Estado.pulando;
            Vector3 fall = RB.velocity;
            fall += Vector3.up * Physics2D.gravity.y * (RB.gravityScale + 10) * fallSpeed * Time.deltaTime;
            RB.velocity = fall;
        }
    }

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
