using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableMethods;
using MessagePack;

[SerializeClassMethods]
public class SerializationTester : MonoBehaviour
{
    public void RunTest()
    {
        var input = new PrimitiveInput()
        {
            s = "wpeior",
            i = 1,
            f = 5.5f,
            d = 8.8d,
            b = true,
        };
        var message = MessagePackSerializer.Serialize(input);

        var output = MessagePackSerializer.Deserialize<PrimitiveOutput>(message);
        Debug.Log(JsonUtility.ToJson(output,true));
    }
}
