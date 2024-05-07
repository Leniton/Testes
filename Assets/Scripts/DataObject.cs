using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataObject : MonoBehaviour
{
    [SerializeField] private TMP_Text idText;

    public void SetData(Data data)
    {
        gameObject.name = $"DataObject ( id: {data.id} )";
        idText.text = data.id.ToString();
    }
}

public struct Data
{
    public int id;
}