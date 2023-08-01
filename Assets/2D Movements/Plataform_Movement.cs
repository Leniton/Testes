using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Plataform_Movement : MonoBehaviour {

    /*
    setting up:
    - adicionar Tag "Chão"
    - criar layers "Não Controlado" na posição 3 e "Chão"
    */

    public enum Estado { parado, andando, pulando }
    Rigidbody2D RB;

    //[HideInInspector] 
    public bool left, right, jump;

    [SerializeField] public Estado estado = new Estado();
    [SerializeField] bool fliped;

    [Header("Movement parapeters")]
    [SerializeField] bool controleMovimento = true;
    [SerializeField] float startingSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float topSpeed = 5;
    [SerializeField] float deceleration;

    [Header("Gravity parameters")]
    [SerializeField] float gravity = .8f;
    [SerializeField] float terminalVelocity = 10;

    [Header("Jump parapeters")]
    [SerializeField] float pulo = 10;
    [SerializeField] bool puloCurto;
    [SerializeField,Range(.01f,.8f),Tooltip("At wich point in the jump you start falling with the short jump")] 
    float fallSpot;

    
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
        Gravidade();
        Mover();
    }

    void Gravidade()
    {
        if (nochao) return;

        if(puloCurto && fallSpot > 0 && !jump && RB.velocity.y > 0 && 
            RB.velocity.y <= pulo * Mathf.Abs(fallSpot-1))
        {
            Vector2 dropOff = Vector2.one;
            dropOff.y = .2f;
            RB.velocity *= dropOff;
        }

        Vector2 gravityEffect = Vector2.zero;
        if (RB.velocity.y > -terminalVelocity)
            gravityEffect.y -= gravity;

        RB.velocity += gravityEffect;

        if (RB.velocity.y < 0)
        {
            jump = false;
            estado = Estado.pulando;
        }
    }

    void Mover()
    {
        if (controleMovimento)
        {
            if (jump && pulo > 0)
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
                if(!puloCurto)
                jump = false;
            }
            if (left)
            {
                switch (estado)
                {
                    case Estado.parado:
                        estado = Estado.andando;
                        float startSpeed = topSpeed;
                        if (acceleration > 0) startSpeed = startingSpeed;

                        RB.velocity = Vector3.left * startSpeed;
                        break;
                    case Estado.andando:

                        float currentSpeed = topSpeed;

                        if (acceleration > 0)
                        {
                            currentSpeed = -RB.velocity.x;
                            currentSpeed = Mathf.Clamp(currentSpeed, startingSpeed, topSpeed);
                            currentSpeed = Mathf.Clamp(currentSpeed + acceleration, startingSpeed, topSpeed);
                        }

                        RB.velocity = Vector3.left * currentSpeed;

                        if (!nochao) 
                        {
                            estado = Estado.pulando;
                        }
                        break;
                    case Estado.pulando:
                        RB.velocity = new Vector3(-1 * topSpeed, RB.velocity.y, 0);
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
                        float startSpeed = topSpeed;
                        if (acceleration > 0) startSpeed = startingSpeed;

                        RB.velocity = Vector3.right * startSpeed;
                        break;
                    case Estado.andando:
                        float currentSpeed = topSpeed;

                        if (acceleration > 0)
                        {
                            currentSpeed = RB.velocity.x;
                            currentSpeed = Mathf.Clamp(currentSpeed, startingSpeed, topSpeed);
                            currentSpeed = Mathf.Clamp(currentSpeed + acceleration, startingSpeed, topSpeed);
                        }

                        RB.velocity = Vector3.right * currentSpeed;

                        if (!nochao)
                        {
                            estado = Estado.pulando;
                        }
                        break;
                    case Estado.pulando:
                        RB.velocity = new Vector3(1 * topSpeed, RB.velocity.y, 0);
                        //print("no ar");
                        break;
                }
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                float decrease = RB.velocity.x - (Mathf.Sign(RB.velocity.x) * deceleration);

                Vector2 currentSpeed = Vector2.zero;
                currentSpeed.y = RB.velocity.y;
                if (deceleration > 0 && (RB.velocity.x < -0.00001 || RB.velocity.x > 0.00001))
                    currentSpeed.x = decrease;

                if (nochao)
                {
                    currentSpeed.y = 0;
                    estado = Estado.parado;
                }
                else
                {
                    estado = Estado.pulando;
                }

                RB.velocity = currentSpeed;
            }
        }
        else
        {
            if (nochao)
            {
                RB.velocity = Vector2.zero;
                estado = Estado.parado;
            }
            else
            {
                RB.velocity = new Vector2(0, RB.velocity.y);
                estado = Estado.pulando;
            }
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

    public void MoveControl(bool inControl)
    {
        controleMovimento = inControl;

        gameObject.layer = inControl ? 0 : 3;
    }

    public void ResetInput()
    {
        left = false;
        right = false;
        jump = false;
    }
}
