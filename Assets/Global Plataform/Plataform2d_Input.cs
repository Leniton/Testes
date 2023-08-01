using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform2d_Input : MonoBehaviour
{

    Plataform_Script plataform;

    void Awake()
    {
        plataform = GetComponent<Plataform_Script>();
        if (plataform) plataform.GetComponentInChildren<Plataform_Script>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            plataform.input.y = 1;
        }
    }
}
