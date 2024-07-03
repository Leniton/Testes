using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AddressableAsyncInstances
{
    public class AAComponent<T> where T : Component
    {
        private T component;
        private Queue<Action<T>> actionQueue;
        private AssetReference reference;

        public AAComponent(string address, Transform parent = null)
        {
            component = null;
            actionQueue = new();
            reference = new(address);
            Addressables.InstantiateAsync(reference, parent).Completed += EmptyQueue;
        }

        public AAComponent(GameObject instance)
        {
            component = instance.GetComponent<T>();
        }

        private void EmptyQueue(AsyncOperationHandle<GameObject> handle)
        {
            component = handle.Result.GetComponent<T>();
            while (actionQueue.Count > 0)
            {
                Action<T> current = actionQueue.Dequeue();
                current?.Invoke(component);
                if (current == DestroyAsyncObject)
                {
                    Debug.LogWarning("object destroyed, canceling further actions");
                    break;
                }
            }
        }
        public void QueueAction(Action<T> action)
        {
            if (component == null)
                actionQueue.Enqueue(action);
            else
                action?.Invoke(component);
        }

        public void Destroy()
        {
            QueueAction(DestroyAsyncObject);
        }

        private void DestroyAsyncObject(T _component)
        {
            reference.ReleaseInstance(_component.gameObject);
            actionQueue.Clear();
        }
    }
}