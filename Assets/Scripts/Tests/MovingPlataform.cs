using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lenix.NumberUtilities;
using PhysicsHelper;

public class MovingPlataform : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float cycleDuration = 5;
    private PhysicsHandler physicsHandler;

    private void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();
    }

    private void Update()
    {
        float direction = NumberUtil.SineWave(Time.time, 1, 1f/cycleDuration);
        physicsHandler.Velocity = Vector3.right * direction * speed;
    }
}
