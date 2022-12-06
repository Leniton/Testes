using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderData
{
    public GameObject gameObject;
    public Bounds bounds;

    public ColliderData(Collider collider)
    {
        gameObject = collider.gameObject;
        bounds = collider.bounds;
    }

    public ColliderData(Collider2D collider)
    {
        gameObject = collider.gameObject;
        bounds = collider.bounds;
    }
}
