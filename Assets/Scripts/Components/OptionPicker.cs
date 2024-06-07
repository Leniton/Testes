using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPicker : MonoBehaviour
{
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private Button button;
    [SerializeField] private RectTransform content;
    [SerializeField] private OptionItem template;

    public Action<int> onPick;

    [HideInInspector] public List<OptionItem> options = new();

    private void Awake()
    {
        if (!scroll) scroll = GetComponentInChildren<ScrollRect>();
        if (!button) button = GetComponentInChildren<Button>();
        if (!template) template = GetComponentInChildren<OptionItem>();

        template.gameObject.SetActive(false);
        CloseOptions();
        button.onClick.AddListener(OpenOptions);
    }

    public void OpenOptions()
    {
        if (scroll.gameObject.activeInHierarchy)
        {
            CloseOptions();
            return;
        }
        scroll.gameObject.SetActive(true);
    }
    
    public void CloseOptions()
    {
        scroll.gameObject.SetActive(false);
    }

    public void SetOptions(List<string> _options)
    {
        ClearOptions();

        for (int i = 0; i < _options.Count; i++)
        {
            int id = i;
            OptionItem item = Instantiate(template, content);
            item.gameObject.SetActive(true);
            item.SetUp(_options[id]);
            item.OnClick += () => PickOption(id);
            options.Add(item);
        }
    }

    private void PickOption(int id)
    {
        onPick?.Invoke(id);
        CloseOptions();
    }

    public void ClearOptions()
    {
        int childs = options.Count;
        for (int i = 0; i < childs; i++)
        {
            Destroy(content.GetChild(0).gameObject);
        }
        options.Clear();
    }
}
