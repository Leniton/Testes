using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Joint2D))]
public class BallScript : MonoBehaviour
{
    private Joint2D joint;
    private Collider2D collider;
    private bool connected => joint.connectedBody != null;
    private void Awake()
    {
        joint = GetComponent<Joint2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (connected) return;
        if (!other.TryGetComponent(out CarMovement car)) return;
        joint.connectedBody = car.GetComponent<Rigidbody2D>();
        joint.enabled = true;
        collider.enabled = true;
    }

    private void OnJointBreak2D(Joint2D brokenJoint)
    {
        collider.enabled = false;
    }
}
