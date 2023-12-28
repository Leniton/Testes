using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTester : MonoBehaviour
{
    PhysicsHandler physicsHandler;
    float prevCheckpointTime = 0;

    private void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();
        physicsHandler.TriggerEnter += CheckSpeed;
    }

    private void CheckSpeed(ColliderData data)
    {
        Debug.Log($"hit at {physicsHandler.GetVelocity()} speed on {Time.time - prevCheckpointTime} seconds");
        prevCheckpointTime = Time.time;
    }
}
