using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEditor;
using Object = UnityEngine.Object;

public class MethodTestData
{
    private string path;
    private string filePath;
    private Dictionary<string, object> methodData;
    public Dictionary<string, object> methodParameters
    {
        get
        {
            if (methodData == null) methodData = Load();
            return methodData;
        }
    }

    public MethodTestData()
    {
        path = Directory.GetCurrentDirectory()+"\\Temp\\";
        filePath = path + "methodTestData.json";
        //Debug.Log(Directory.GetFiles(path).Length);
    }

    private async Task SaveChanges()
    {
        await Task.Delay(400);
        Save();
    }

    Dictionary<string, object> Load()
    {
        Dictionary<string, object> data = new();
        if (File.Exists(filePath))
        {
            DataContractJsonSerializer son = new DataContractJsonSerializer(typeof(Dictionary<string, object>));
            FileStream file = new FileStream(filePath, FileMode.Open);
            file.Position = 0;
            data = (Dictionary<string, object>)son.ReadObject(file);

            file.Close();
            data = DeserializeObjects(data);
            return data;
        }

        Debug.LogError("não encontrado");
        return data;
    }
 
    private Dictionary<string,object> DeserializeObjects(Dictionary<string,object> data)
    {
        Dictionary<string, object> returnData = new Dictionary<string, object>();
        foreach (KeyValuePair<string,object> item in data)
        {
            try
            {
                returnData[item.Key] = JsonUtility.FromJson<ObjectID>((string)item.Value).ToObject();
            }
            catch (Exception e)
            {
                returnData.Add(item.Key,item.Value);
            }
        }

        return returnData;
    }
    
    public void Save()
    {
        DataContractJsonSerializer son = new DataContractJsonSerializer(typeof(Dictionary<string,object>));
        //Debug.Log(filePath);
        FileStream file = new FileStream(filePath, FileMode.Create);
        son.WriteObject(file, SerializableDictionary());
        file.Close();
    }

    private Dictionary<string, object> SerializableDictionary()
    {
        Dictionary<string, object> returnType = new();

        foreach (KeyValuePair<string,object> item in methodData)
        {
            try
            {
                if (item.Value != null)
                    returnType.Add(item.Key, JsonUtility.ToJson(new ObjectID((UnityEngine.Object)item.Value)));
                else returnType.Add(item.Key, item.Value);
            }
            catch (Exception e)
            {
                returnType.Add(item.Key, item.Value != null ? item.Value : null);
            }
        }
        
        return returnType;
    }
 
    public void DeleteSave()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}

[System.Serializable]
public class ObjectID
{
    public int id;
    public string name;
    public ObjectID(Object obj)
    {
        id = obj.GetInstanceID();
        name = obj.name;
    }

    public Object ToObject() => EditorUtility.InstanceIDToObject(id);
}
