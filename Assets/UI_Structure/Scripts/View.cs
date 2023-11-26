using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class View
{
    TypingScript typer;

    public void Init(Transform parent)
    {
        GameObject typeScript = (GameObject)Object.Instantiate(Resources.Load("UI_Structure/TextTyper"), parent);
        typer = typeScript.GetComponent<TypingScript>();
    }

    public void Type(string title = "title\n", string text = "")
    {
        TextToType textToType = new TextToType();
        textToType.StartText = title;
        textToType.ToTypeText = text;
        typer.Type(textToType);
    }
}
