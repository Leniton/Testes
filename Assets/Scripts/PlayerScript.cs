using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Transform lastCheckpoint;

    public static event Action OnPlayerDeath;

    private void Awake()
    {
        OnPlayerDeath += () => transform.position = lastCheckpoint.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Respawn"))
            lastCheckpoint = other.transform;
        else if (other.CompareTag("enemy")) KillPlayer();
    }
    
    public static void KillPlayer() => OnPlayerDeath?.Invoke();
}