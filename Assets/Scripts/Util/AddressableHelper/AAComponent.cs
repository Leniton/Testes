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

        /// <summary>
        /// Loads GameObject with component at address
        /// </summary>
        /// <param name="address">The addressable path</param>
        public AAComponent(string address)
        {
            Setup(address).Completed += EmptyQueue;
        }
        
        /// <summary>
        /// Loads GameObject with component at address and instantiate it
        /// </summary>
        /// <param name="address"></param>
        /// <param name="parent"></param>
        public AAComponent(string address, Transform parent)
        {
            Setup(address).Completed += _ => Addressables.InstantiateAsync(reference, parent).Completed += EmptyQueue;
        }

        private AsyncOperationHandle<GameObject> Setup(string address)
        {
            component = null;
            actionQueue = new();
            reference = new(address);
            var operation = Addressables.LoadAssetAsync<GameObject>(address);
            operation.ReleaseHandleOnCompletion();
            return operation;
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

        public static void LoadComponent(string address, Action<T> onLoaded) =>
            new AAComponent<T>(address).QueueAction(onLoaded);

        public static void InstantiateComponent(string address, Action<T> onLoaded, Transform parent = null) =>
            new AAComponent<T>(address, parent).QueueAction(onLoaded);
    }
}