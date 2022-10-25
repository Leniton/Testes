using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsHandler))]
public class Plataform_Script : MonoBehaviour
{
#if UNITY_EDITOR
    bool testMode = true;
#endif

    public Vector3 input = Vector3.zero;
    public bool hasControl = true;

    //Reference Parameters
    PhysicsHandler physicsHandler;

    //Jump parameters
    [SerializeField] float jumpHeight;
    [SerializeField] float timeToMaxHeight;
    [SerializeField] float timeToFall;
    float jumpStrength;
    float gravity;
    float terminalVelocity;

    //Movement parameters
    Vector3 finalVelocity;

    void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();
        
        CalculateParameters();
    }

    void CalculateParameters()
    {
        jumpStrength = (jumpHeight / timeToMaxHeight);
        gravity = (jumpHeight + (jumpStrength*timeToMaxHeight) - jumpHeight) / (Mathf.Pow(timeToMaxHeight,2)/2);
    }


    void FixedUpdate()
    {
        if (hasControl)
        {
            if(input.y > 0)
            {
                Jump();
                input.y = 0;
                physicsHandler.SetVelocity(finalVelocity);
            }
        }

    }

    void Jump()
    {
        print(gravity);

        finalVelocity.y = jumpStrength;
    }
}
