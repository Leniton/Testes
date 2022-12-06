using System;
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

    public Action<CollisionData> CollisionEnter, CollisionStay, CollisionExit;
    public Action<ColliderData> TriggerEnter, TriggerStay, TriggerExit;

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

        velocity = finalValue;
        return velocity;
    }

    #region Collision

    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter?.Invoke(new CollisionData(collision));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEnter?.Invoke(new CollisionData(collision));
    }

    private void OnCollisionStay(Collision collision)
    {
        CollisionStay?.Invoke(new CollisionData(collision));
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        CollisionStay?.Invoke(new CollisionData(collision));
    }

    private void OnCollisionExit(Collision collision)
    {
        CollisionExit?.Invoke(new CollisionData(collision));
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        CollisionExit?.Invoke(new CollisionData(collision));
    }

    private void OnTriggerEnter(Collider collision)
    {
        TriggerEnter?.Invoke(new ColliderData(collision));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEnter?.Invoke(new ColliderData(collision));
    }

    private void OnTriggerStay(Collider collision)
    {
        TriggerStay?.Invoke(new ColliderData(collision));
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        TriggerStay?.Invoke(new ColliderData(collision));
    }

    private void OnTriggerExit(Collider collision)
    {
        TriggerExit?.Invoke(new ColliderData(collision));
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        TriggerExit?.Invoke(new ColliderData(collision));
    }

    #endregion
}