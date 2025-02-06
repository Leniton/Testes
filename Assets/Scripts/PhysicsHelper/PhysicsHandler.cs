using System;
using UnityEngine;

namespace PhysicsHelper
{
    public abstract class PhysicsHandler : MonoBehaviour
    {
        private Vector3 velocity;
        public abstract Vector3 Velocity { get; set; }

        public Action<CollisionData> CollisionEnter, CollisionStay, CollisionExit;
        public Action<ColliderData> TriggerEnter, TriggerStay, TriggerExit;
    }
}