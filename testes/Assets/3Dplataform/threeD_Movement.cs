using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class threeD_Movement : MonoBehaviour
{
    //Apenas a base para o movimento, usar outro script para os inputs

    public enum Estado { parado, andando, pulando }

    [HideInInspector]
    public bool left, right, foward, backward, jump;

    [SerializeField] public Estado estado = new Estado();

    public bool controleMovimento = true;
    [SerializeField] protected float vel;
    [SerializeField] protected float NoFowMul;
    [SerializeField] protected float Gravidade = 10;
    [SerializeField] protected int pulo;
    [SerializeField] protected bool puloCurto;
    [SerializeField] protected float fallSpeed;

    int frontBack, leftRight;

    protected Rigidbody RB;

    public Collider chaoPisado { get; private set; }
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

            if (jump)
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
                jump = false;
            }
            if (foward)
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        //RB.velocity = transform.forward * vel;
                        frontBack = 1;
                        break;
                    case Estado.andando:
                        /*if (RB.velocity.x > 0)
                        {
                            RB.velocity = Vector3.left * vel;
                        }*/
                        //RB.velocity = transform.forward * vel;
                        frontBack = 1;
                        break;
                    case Estado.pulando:
                        //RB.velocity = new Vector3(transform.forward.x * vel, RB.velocity.y, transform.forward.z * vel);
                        frontBack = 1;
                        break;
                }
            }
            else if (backward)
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        //RB.velocity = -transform.forward * vel;
                        frontBack = -1;
                        break;
                    case Estado.andando:
                        /*if (RB.velocity.x > 0)
                        {
                            RB.velocity = Vector3.left * vel;
                        }*/
                        //RB.velocity = -transform.forward * vel;
                        frontBack = -1;
                        break;
                    case Estado.pulando:
                        //RB.velocity = new Vector3(-transform.forward.x * vel, RB.velocity.y, -transform.forward.z * vel);
                        frontBack = -1;
                        break;
                }
            }
            else
            {
                frontBack = 0;
            }

            if (left)
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        //RB.velocity = -transform.right * vel;
                        leftRight = -1;
                        break;
                    case Estado.andando:

                        //RB.velocity = -transform.right * vel;
                        leftRight = -1;
                        break;
                    case Estado.pulando:
                        //RB.velocity = new Vector3(-transform.right.x * vel, RB.velocity.y, -transform.right.z * vel);
                        leftRight = -1;
                        break;
                }
            }
            else if (right)
            {
                //print(estado);
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        //RB.velocity = transform.right * vel;
                        leftRight = 1;
                        break;
                    case Estado.andando:

                        //RB.velocity = transform.right * vel;
                        leftRight = 1;
                        break;
                    case Estado.pulando:
                        //RB.velocity = new Vector3(transform.right.x * vel, RB.velocity.y, transform.right.z * vel);
                        //print("no ar");
                        leftRight = 1;
                        break;
                }
            }
            else
            {
                leftRight = 0;
            }

            //print((foward == backward == left == right) && foward == false);
            if (right == false && foward == false && backward == false && left == false)
            {
                if (!Momentunm)
                {
                    if (nochao)
                    {
                        RB.velocity = Vector2.zero;
                        frontBack = leftRight = 0;
                        estado = Estado.parado;
                    }
                    else
                    {
                        RB.velocity = new Vector3(0, RB.velocity.y, 0); ;
                        estado = Estado.pulando;
                    }
                }
            }
            else if(estado != Estado.parado)
            {
                //print(frontBack + " | " + leftRight);
                Vector3 movement = (transform.forward * vel * frontBack) + (transform.right * vel * leftRight);
                movement = new Vector3(movement.x, RB.velocity.y, movement.z);
                RB.velocity = movement;

                //if (estado == Estado.pulando && leftRight != 0) print("assim?");
            }

        }

        if (fallSpeed > 0 && (RB.velocity.y < 0 || (puloCurto && !jump && !nochao)))
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

                if (foward || backward
                  || left || right)
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
                chaoPisado = c.collider;
            }
        }
    }

    private void OnCollisionExit(Collision c)
    {
        if (c.collider == chaoPisado)
        {
            nochao = false;
        }
    }
}
