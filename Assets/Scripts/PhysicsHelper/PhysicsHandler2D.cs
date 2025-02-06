using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsHelper
{
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsHandler2D : PhysicsHandler
    {
        private Rigidbody2D rb;
        public override Vector3 Velocity { get => rb.velocity; set => rb.velocity = value; }

        private void Reset()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
            rb ??= GetComponent<Rigidbody2D>();
        }

        #region Collision
        private void OnCollisionEnter2D(Collision2D collision) => CollisionEnter?.Invoke(new CollisionData(collision));
        private void OnCollisionStay2D(Collision2D collision) => CollisionStay?.Invoke(new CollisionData(collision));
        private void OnCollisionExit2D(Collision2D collision) => CollisionExit?.Invoke(new CollisionData(collision));

        private void OnTriggerEnter2D(Collider2D collision) => TriggerEnter?.Invoke(new ColliderData(collision));
        private void OnTriggerStay2D(Collider2D collision) => TriggerStay?.Invoke(new ColliderData(collision));
        private void OnTriggerExit2D(Collider2D collision) => TriggerExit?.Invoke(new ColliderData(collision));
        #endregion
    }
}