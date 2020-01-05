using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class threeD_Movement : MonoBehaviour{

    enum Estado { parado, andando, pulando }

    [SerializeField] Estado estado = new Estado();

    [SerializeField] bool controleMovimento = true;
    [SerializeField] int vel;
    [SerializeField] float NoFowMul;
    [SerializeField] float Gravidade = 10;
    [SerializeField] int pulo;
    [SerializeField] bool puloCurto;
    [SerializeField] float fallSpeed;
    Rigidbody RB;

    bool nochao;

    void Start()
    {
        estado = Estado.parado;
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(true/*estado == Estado.pulando*/)
        {
            RB.AddForce(Vector3.down * Gravidade);
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
                        RB.velocity = new Vector3(transform.forward.x * vel, RB.velocity.y, transform.forward.z);
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
                        RB.velocity = new Vector3(-transform.right.x * vel, RB.velocity.y, -transform.right.z);
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
                        RB.velocity = new Vector3(transform.right.x * vel, RB.velocity.y, transform.right.z);
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
                        RB.velocity = new Vector3(-transform.forward.x * vel, RB.velocity.y, -transform.forward.z);
                        break;
                }
            }
            else
            {
                RB.velocity = new Vector2(0, RB.velocity.y);
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
        else if (RB.velocity.y == 0)
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
        }
    }
    void OnCollisionEnter(Collision c)
    {
        print("normal:" + c.GetContact(0).normal);
        if (c.collider.tag == "Chão")
        {
            if (c.GetContact(0).normal == Vector3.up)
            {
                RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
                /*if (RB.velocity.x != 0)
                {
                    estado = Estado.andando;
                    //print("desceu andando");
                }
                else if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                {
                    estado = Estado.parado;
                    //print("desceu");
                }*/
                nochao = true;
            }
        }
    }

}
