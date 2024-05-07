using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInstancer : MonoBehaviour
{
    public float dataHeight { get; private set; }
    private DataObject dataTemplate;

    private Queue<DataObject> instanceQueue;
    private Queue<DataObject> savedQueue;

    private void Awake()
    {
        dataTemplate = GetComponentInChildren<DataObject>();
        dataHeight = (dataTemplate.transform as RectTransform).rect.height;

        savedQueue = new Queue<DataObject>();
        instanceQueue = new Queue<DataObject>();
    }

    public DataObject GetInstance(Data data)
    {
        DataObject instance = null;
        if (savedQueue.Count > 0)
        {
            instance = savedQueue.Dequeue();
            instance.gameObject.SetActive(true);
        }
        else instance = Instantiate(dataTemplate);

        instanceQueue.Enqueue(instance);
        instance.SetData(data);
        return instance;
    }

    public void Dispose()
    {
        DataObject instance = instanceQueue.Dequeue();
        savedQueue.Enqueue(instance);
        instance.gameObject.SetActive(false);
    }

    public void DisposeAll()
    {
        while (instanceQueue.Count > 0)
        {
            savedQueue.Enqueue(instanceQueue.Dequeue());
        }
    }

    //pretend it gets from a datababe
    public Data GetData(int id)
    {
        Data data = new();
        data.id = id;
        return data;
    }
}
