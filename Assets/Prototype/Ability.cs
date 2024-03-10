using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Plataform2d_Input plataformInput;
    protected Plataform_Script plataform;
    protected PhysicsHandler physicsHandler;
    protected Vector2 currentInput = Vector2.zero;
    protected float lastXinput = 1;

    protected virtual void Awake()
    {
        plataform = GetComponent<Plataform_Script>();
        physicsHandler = GetComponent<PhysicsHandler>();
        plataformInput = GetComponent<Plataform2d_Input>();
    }

    protected virtual void Update()
    {
        Vector2 input = currentInput;
        if (Input.GetKeyDown(KeyCode.W))
        {
            input.y += 1;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            input.y -= 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            input.y -= 1;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            input.y += 1;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            input.x -= 1;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            input.x += 1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            input.x += 1;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            input.x -= 1;
        }

        currentInput = input;

        if (input == Vector2.zero)
        {
            input.x = lastXinput;
        }

        if (input.x != 0) lastXinput = input.x;

        if (plataform.levelOfControl <= 0) return;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TriggerAbility(input);
        }
    }

    protected abstract void TriggerAbility(Vector3 direction);
}
