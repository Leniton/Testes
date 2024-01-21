using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrigger : MonoBehaviour
{
    [SerializeField] Dash dash = new();
    Plataform_Script plataform;
    Vector2 currentInput = Vector2.zero;
    float lastXinput = 1;

    private void Awake()
    {
        plataform = GetComponent<Plataform_Script>();

        PhysicsHandler physicsHandler = GetComponent<PhysicsHandler>();
        dash.Init(physicsHandler);
        dash.doneDashing += () =>
        {
            plataform.levelOfControl = 1;
            plataform.useGravity = true;

            Debug.DrawRay(transform.position, Vector3.right, Color.magenta, 1);
            Debug.DrawRay(transform.position, Vector3.left, Color.magenta, 1);
            Debug.DrawRay(transform.position, Vector3.up, Color.magenta, 1);
            Debug.DrawRay(transform.position, Vector3.down, Color.magenta, 1);

            Debug.DrawRay(transform.position, (Vector3.up + Vector3.right).normalized, Color.magenta, 1);
            Debug.DrawRay(transform.position, (Vector3.up + Vector3.left).normalized, Color.magenta, 1);
            Debug.DrawRay(transform.position, (Vector3.down + Vector3.right).normalized, Color.magenta, 1);
            Debug.DrawRay(transform.position, (Vector3.down + Vector3.left).normalized, Color.magenta, 1);
        };
    }

    void Update()
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

        dash.CalculateParameters();//test only
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            plataform.levelOfControl = 0;
            plataform.useGravity = false;
            dash.StartDash(input);
        }
    }
}
