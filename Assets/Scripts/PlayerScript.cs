using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Transform lastCheckpoint;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Respawn"))
        {
            lastCheckpoint = other.transform;
        }
        else if (other.CompareTag("enemy"))
        {
            transform.position = lastCheckpoint.position;
        }
    }
}