using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Plataform_Movement))]
public class PlayerController : MonoBehaviour
{
    Plataform_Movement mov;
    Animator anim;

    void Start()
    {
        mov = GetComponent<Plataform_Movement>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        //quando pressionar
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            mov.jump = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            mov.left = true;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            mov.right = true;
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

        if (anim == null) return;

        //animações
        if(mov.estado == Plataform_Movement.Estado.andando)
        {
            anim.SetBool("Walking", true);
        }else if (mov.estado == Plataform_Movement.Estado.parado)
        {
            anim.SetBool("Walking", false);
        }
    }
}
