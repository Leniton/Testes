using System;
using UnityEngine;
using UnityEngine.UI;

public class SideOption : MonoBehaviour
{
    public Action<SideData> OnSelect;
    [SerializeField] private Button button;
    [SerializeField] private Image side;
    [SerializeField] private Image pip;
    private SideData data;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetUp(SideData newData, int pips = 0)
    {
        data = newData;

        side.sprite = data.sprite;
        pip.sprite = DiceSideDatabase.pips[data.pips >= 0 ? Mathf.Clamp(pips,0,DiceSideDatabase.pips.Length - 2) : DiceSideDatabase.pips.Length - 1];
    }

    private void OnClick()
    {
        OnSelect?.Invoke(data);
    }
}
