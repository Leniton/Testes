using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class threeD_Movement : MonoBehaviour
{

    protected enum Estado { parado, andando, pulando }

    [SerializeField] protected Estado estado = new Estado();

    public bool controleMovimento = true;
    [SerializeField] protected float vel;
    [SerializeField] protected float NoFowMul;
    [SerializeField] protected float Gravidade = 10;
    [SerializeField] protected int pulo;
    [SerializeField] protected bool puloCurto;
    [SerializeField] protected float fallSpeed;

    protected Rigidbody RB;

    [SerializeField] protected bool nochao;
    [Tooltip("Forças aplicadas se mantém")] [SerializeField] bool Momentunm = true;

    void Start()
    {
        estado = Estado.parado;
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (true/*estado == Estado.pulando*/)
        {
            RB.AddForce(Vector3.down * Gravidade);
        }

        if ((RB.velocity.y > 0.1f || RB.velocity.y < -0.1f) &&
            estado != Estado.pulando && controleMovimento)
        {
            estado = Estado.pulando;
        }

        if (controleMovimento)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.pulando;
                        RB.AddForce(Vector2.up * pulo, ForceMode.Impulse);
                        nochao = false;
                        break;
                    case Estado.andando:
                        estado = Estado.pulando;
                        RB.AddForce(Vector2.up * pulo, ForceMode.Impulse);
                        nochao = false;
                        break;
                    case Estado.pulando:

                        break;
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        RB.velocity = transform.forward * vel;
                        break;
                    case Estado.andando:
                        /*if (RB.velocity.x > 0)
                        {
                            RB.velocity = Vector3.left * vel;
                        }*/
                        RB.velocity = transform.forward * vel;
                        break;
                    case Estado.pulando:
                        RB.velocity = new Vector3(transform.forward.x * vel, RB.velocity.y, transform.forward.z * vel);
                        break;
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        RB.velocity = -transform.right * vel;
                        break;
                    case Estado.andando:

                        RB.velocity = -transform.right * vel;
                        break;
                    case Estado.pulando:
                        RB.velocity = new Vector3(-transform.right.x * vel, RB.velocity.y, -transform.right.z * vel);
                        break;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //print(estado);
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        RB.velocity = transform.right * vel;
                        break;
                    case Estado.andando:

                        RB.velocity = transform.right * vel;
                        break;
                    case Estado.pulando:
                        RB.velocity = new Vector3(transform.right.x * vel, RB.velocity.y, transform.right.z * vel);
                        //print("no ar");
                        break;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        RB.velocity = -transform.forward * vel;
                        break;
                    case Estado.andando:
                        /*if (RB.velocity.x > 0)
                        {
                            RB.velocity = Vector3.left * vel;
                        }*/
                        RB.velocity = -transform.forward * vel;
                        break;
                    case Estado.pulando:
                        RB.velocity = new Vector3(-transform.forward.x * vel, RB.velocity.y, -transform.forward.z * vel);
                        break;
                }
            }
            else
            {
                if (!Momentunm)
                {
                    RB.velocity = new Vector3(0, RB.velocity.y, 0);
                }
            }

        }

        if (fallSpeed > 0 && (RB.velocity.y < 0 || (puloCurto && !Input.GetKey(KeyCode.W) && !nochao)))
        {
            print("caindo");
            estado = Estado.pulando;
            Vector3 fall = RB.velocity;
            fall += Vector3.up * Physics.gravity.y * (10 * Gravidade) * fallSpeed * Time.deltaTime;
            RB.velocity = fall;
        }
        /*else if (RB.velocity.y == 0)
        {
            if ((RB.velocity.x >= 0.1f && RB.velocity.x <= 0.1f)
              ||(RB.velocity.z >= 0.1f && RB.velocity.z <= 0.1f))
            {
                print("andando");
                estado = Estado.andando;
                //print("desceu andando");
            }
            else if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)
                  && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                estado = Estado.parado;
                //print("desceu");
            }
            nochao = true;
        }*/


    }

    protected virtual void OnCollisionEnter(Collision c)
    {
        //print("normal:" + c.GetContact(0).normal);
        //print(c.GetContact(0).normal == Vector3.up);
        if (c.collider.tag == "Chão")
        {
            if (c.GetContact(0).normal.y >= 0.4f)
            {
                RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);

                if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)
                 && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
                {
                    print("andando");
                    estado = Estado.andando;
                    //print("desceu andando");
                }
                else /*if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)
                      && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))*/
                {
                    estado = Estado.parado;
                    //print("desceu");
                }
                nochao = true;
            }
        }
    }

}
