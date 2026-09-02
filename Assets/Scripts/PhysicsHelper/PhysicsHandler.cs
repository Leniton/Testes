using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsHelper
{
    public abstract class PhysicsHandler : MonoBehaviour
    {
        public abstract Vector3 Velocity { get; set; }
        
        private List<AppliedForce> appliedForces = new();

        public Action<CollisionData> CollisionEnter, CollisionStay, CollisionExit;
        public Action<ColliderData> TriggerEnter, TriggerStay, TriggerExit;

        protected Vector3 CalculateVelocities(Vector3? baseVelocity = null)
        {
            var newVelocity = baseVelocity ?? Vector3.zero;

            for (int i = 0; i < appliedForces.Count; i++)
                newVelocity += appliedForces[i].ForceDirection;
            
            return newVelocity;
        }

        public AppliedForce ApplyForce(Vector3 forceDirection, float forceMultiplier = 1)
        {
            AppliedForce force = new(forceDirection, forceMultiplier);
            force.OnValueChanged += RecalculateVelocities;
            appliedForces.Add(force);
            RecalculateVelocities();
            return force;
        }

        public void RemoveForce(AppliedForce force, float fadeDuration = 0)
        {
            if (!appliedForces.Contains(force)) return;
            if (fadeDuration <= 0) InternalRemoveForce(force);
            else StartCoroutine(FadeForce(force, fadeDuration));
        }

        private IEnumerator FadeForce(AppliedForce force, float fadeDuration)
        {
            float startingForce = force.Force;
            float time = 0;
            while (time < fadeDuration)
            {
                var scaledTime = time / fadeDuration;
                force.Force = Mathf.Lerp(startingForce, 0, scaledTime);
                yield return null;
                time += Time.deltaTime;
            }

            force.Force = 0;
            InternalRemoveForce(force);
        }

        protected virtual void InternalRemoveForce(AppliedForce force)
        {
            force.OnValueChanged -= RecalculateVelocities;
            appliedForces.Remove(force);
            RecalculateVelocities();
        }

        protected void RecalculateVelocities()
        {
            Velocity = CalculateVelocities();
        }
    }

    [Serializable]
    public class AppliedForce
    {
        private Vector3 _direction;
        private float _force;
        
        public event Action OnValueChanged;

        public Vector3 Direction
        {
            get { return _direction; }
            set
            {
                _direction = value; 
                OnValueChanged?.Invoke();
            }
        }

        public float x
        {
            get => _direction.x;
            set
            {
                _direction.x = value;
                OnValueChanged?.Invoke();
            }
        }

        public float y
        {
            get => _direction.y; 
            set
            {
                _direction.y = value;
                OnValueChanged?.Invoke();
            }
        }
        
        public float Force
        {
            get { return _force; }
            set
            {
                _force = value;
                OnValueChanged?.Invoke();
            }
        }

        public Vector3 ForceDirection => Direction * Force;

        public AppliedForce(Vector3 direction, float force) => UpdateValues(direction, force);

        public void UpdateValues(Vector3 direction, float force)
        {
            _direction = direction;
            _force = force;
            OnValueChanged?.Invoke();
        }
    }
}