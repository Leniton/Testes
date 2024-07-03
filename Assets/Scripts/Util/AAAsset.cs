using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AddressableAsyncInstances
{
    public class AAAsset<T>
    {
        private T component;
        private Queue<Action<T>> actionQueue;

        public AAAsset(string address, Transform parent = null)
        {
            actionQueue = new();
            Addressables.LoadAssetAsync<T>(address).Completed += EmptyQueue;
        }

        private void EmptyQueue(AsyncOperationHandle<T> handle)
        {
            component = handle.Result;
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
            Addressables.Release(component);
            actionQueue.Clear();
        }
    }
}