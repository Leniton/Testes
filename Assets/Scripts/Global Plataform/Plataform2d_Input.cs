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
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            plataform.input.y = 0;
        }

        plataform.input.x = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift)) plataform.levelOfControl = 0;
        else plataform.levelOfControl = Mathf.Clamp01(plataform.levelOfControl + Time.deltaTime);
    }
}
