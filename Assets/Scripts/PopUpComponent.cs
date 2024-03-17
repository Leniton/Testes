using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class PopUpComponent : MonoBehaviour
{
    [SerializeField] GameObject window;
    [SerializeField] TMP_Text text;

    public static PopUpComponent instance;

    private void Awake()
    {
        if(instance != this)
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
            }
            else Destroy(gameObject);
        }
    }

    public void Show(string desc)
    {
        window.SetActive(true);
        text.text = desc;
    }

    public void Hide()
    {
        window.SetActive(false);
    }
}
