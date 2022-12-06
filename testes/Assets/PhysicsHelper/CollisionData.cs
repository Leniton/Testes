using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollisionData
{
    public GameObject gameObject;
    public ColliderData collider, other;
    public Vector2 relativeVelocity;
    public ContactData[] contacts;

    public CollisionData(Collision collision)
    {
        gameObject = collision.gameObject;
        collider = new ColliderData(collision.collider);
        other = new ColliderData(collision.transform.GetComponent<Collider>());
        relativeVelocity = collision.relativeVelocity;
        contacts = new ContactData[collision.contactCount];

        for (int i = 0; i < contacts.Length; i++)
        {
            contacts[i].point = collision.contacts[i].point;
            contacts[i].normal = collision.contacts[i].normal;
            contacts[i].separation = collision.contacts[i].separation;
        }
    }
    public CollisionData(Collision2D collision)
    {
        gameObject = collision.gameObject;
        collider = new ColliderData(collision.collider);
        other = new ColliderData(collision.otherCollider);
        relativeVelocity = collision.relativeVelocity;
        contacts = new ContactData[collision.contactCount];

        for (int i = 0; i < contacts.Length; i++)
        {
            contacts[i].point = collision.contacts[i].point;
            contacts[i].normal = collision.contacts[i].normal;
            contacts[i].separation = collision.contacts[i].separation;
        }
    }
}
