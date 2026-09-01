using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AddressableAsyncInstances
{
    public class AAAsset<T>
    {
        private T asset;
        private Queue<Action<T>> actionQueue;

        public AAAsset(string address, bool releaseOnCompletion = true)
        {
            actionQueue = new();
            var operation = Addressables.LoadAssetAsync<T>(address);
            if (releaseOnCompletion) operation.ReleaseHandleOnCompletion();
            operation.Completed += EmptyQueue;
        }

        private void EmptyQueue(AsyncOperationHandle<T> handle)
        {
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Error loading asset: " + handle.Status);
                return;
            }
            asset = handle.Result;
            while (actionQueue.Count > 0)
            {
                Action<T> current = actionQueue.Dequeue();
                current?.Invoke(asset);
                if (current == DestroyAsyncObject)
                {
                    Debug.LogWarning("object destroyed, canceling further actions");
                    break;
                }
            }
        }
        public void QueueAction(Action<T> action)
        {
            if (asset == null)
                actionQueue.Enqueue(action);
            else
                action?.Invoke(asset);
        }

        public void Destroy()
        {
            QueueAction(DestroyAsyncObject);
        }

        private void DestroyAsyncObject(T _component)
        {
            Addressables.Release(asset);
            actionQueue.Clear();
        }

        private static Dictionary<string, AAAsset<T>> currentlyLoadingAssets = new();
        public static void LoadAsset(string address, Action<T> onLoaded, bool releaseOnCompletion = true)
        {
            if (!currentlyLoadingAssets.TryGetValue(address, out AAAsset<T> asset))
            {
                asset = new AAAsset<T>(address, releaseOnCompletion);
                currentlyLoadingAssets[address] = asset;
            }
            asset.QueueAction(onLoaded);
        }
    }
}