using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrigger : MonoBehaviour
{
    [SerializeField] Dash dash = new();
    Plataform_Script plataform;

    private void Awake()
    {
        plataform = GetComponent<Plataform_Script>();

        PhysicsHandler physicsHandler = GetComponent<PhysicsHandler>();
        dash.Init(physicsHandler);
        dash.doneDashing += () =>
        {
            plataform.levelOfControl = 1;
            plataform.useGravity = true;
        };
    }

    void Update()
    {
        dash.CalculateParameters();//test only
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            plataform.levelOfControl = 0;
            plataform.useGravity = false;
            dash.StartDash(plataform.input);
        }
    }
}
