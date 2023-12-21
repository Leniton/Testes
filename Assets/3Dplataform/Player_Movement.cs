using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(threeD_Movement))]
public class Player_Movement : MonoBehaviour
{

    Animator anim;
    threeD_Movement mov;

    void Start()
    {
        anim = GetComponent<Animator>();
        mov = GetComponent<threeD_Movement>();
    }

    void Update()
    {
        //quando pressionar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mov.jump = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            mov.left = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            mov.right = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            mov.foward = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            mov.backward = true;
        }

        //quando soltar o botão
        if (Input.GetKeyUp(KeyCode.A))
        {
            mov.left = false;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            mov.right = false;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            mov.foward = false;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            mov.backward = false;
        }

        if (anim == null) return;

        //animações
        if (mov.estado == threeD_Movement.Estado.andando)
        {
            anim.SetBool("Walking", true);
        }
        else if (mov.estado == threeD_Movement.Estado.parado)
        {
            anim.SetBool("Walking", false);
        }
    }
}
