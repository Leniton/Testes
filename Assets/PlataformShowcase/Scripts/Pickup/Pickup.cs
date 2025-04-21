using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPickup(collision.gameObject);
    }

    protected abstract void OnPickup(GameObject target);
}
