using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHandler : MonoBehaviour
{
    Rigidbody rb3D;
    Rigidbody2D rb2D;

    Collider c3D;
    Collider2D c2D;

    private Vector3 velocity;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb3D = GetComponent<Rigidbody>();

        c2D = GetComponent<Collider2D>();
        c3D = GetComponent<Collider>();
    }

    public void SetVelocity(Vector3 value)
    {
        Vector3 finalValue = value;

        if (rb2D != null)
        {
            rb2D.velocity = value;
        }
        else if (rb3D != null)
        {
            rb3D.velocity = value;
        }
        else
        {
            finalValue = Vector3.zero;
        }

        velocity = finalValue;
    }

    public Vector3 GetVelocity()
    {
        Vector3 finalValue = velocity;

        if (rb2D != null)
        {
            finalValue = rb2D.velocity;
        }
        else if (rb3D != null)
        {
            finalValue = rb3D.velocity;
        }
        else
        {
            finalValue = Vector3.zero;
        }

        return velocity;
    }
}