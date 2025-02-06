using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsHelper
{
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsHandler3D : PhysicsHandler
    {
        private Rigidbody rb;
        public override Vector3 Velocity { get => rb.velocity; set => rb.velocity = value; }

        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            rb ??= GetComponent<Rigidbody>();
        }

        #region Collision
        private void OnCollisionEnter(Collision collision) => CollisionEnter?.Invoke(new CollisionData(collision));
        private void OnCollisionStay(Collision collision) => CollisionStay?.Invoke(new CollisionData(collision));
        private void OnCollisionExit(Collision collision) => CollisionExit?.Invoke(new CollisionData(collision));

        private void OnTriggerEnter(Collider collision) => TriggerEnter?.Invoke(new ColliderData(collision));
        private void OnTriggerStay(Collider collision) => TriggerStay?.Invoke(new ColliderData(collision));
        private void OnTriggerExit(Collider collision) => TriggerExit?.Invoke(new ColliderData(collision));
        #endregion
    }
}