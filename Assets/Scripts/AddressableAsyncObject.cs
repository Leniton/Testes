using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableAsyncObject
{
    private GameObject gameObject;
    private Queue<Action<GameObject>> actionQueue;
    private AssetReference reference;

    public AddressableAsyncObject(string address, Transform parent = null)
    {
        gameObject = null;
        actionQueue = new();
        reference = new (address);
        Addressables.InstantiateAsync(reference, parent).Completed += EmptyQueue;
    }

    public AddressableAsyncObject(GameObject instance)
    {
        gameObject = instance;
    }

    private void EmptyQueue(AsyncOperationHandle<GameObject> handle)
    {
        gameObject = handle.Result;
        while (actionQueue.Count > 0)
        {
            Action<GameObject> current = actionQueue.Dequeue();
            current?.Invoke(gameObject);
        }
    }

    public void QueueAction(Action<GameObject> action)
    {
        if (gameObject == null)
            actionQueue.Enqueue(action);
        else
            action?.Invoke(gameObject);
    }

    public void Destroy()
    {
        QueueAction((go) =>
        {
            reference.ReleaseInstance(gameObject);
            actionQueue.Clear();
        });
    }
}