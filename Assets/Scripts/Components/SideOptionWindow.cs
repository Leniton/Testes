using System;
using UnityEngine;
using UnityEngine.UI;

public class SideOptionWindow : MonoBehaviour
{
    [SerializeField] private SideOption baseOption;
    [SerializeField] private RectTransform content;
    [SerializeField] private ValueInputField pipValue;
    [SerializeField] private Button confirmButton;
    private SideData currentSide;

    private static SideOptionWindow instance;

    private Action<SideData> onPickCallback;
    
    private void Awake()
    {
        if (instance != this)
        {
            if (instance == null)
            {
                instance = this;
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        
        SetUp(DiceSideDatabase.sidesData[0]);
        pipValue.onValueChanged += ChangePips;
        for (int i = 0; i < DiceSideDatabase.sidesData.Count; i++)
        {
            SideData data = DiceSideDatabase.sidesData[i];

            SideOption newOption = Instantiate(baseOption, content);
            newOption.gameObject.SetActive(true);
            newOption.SetUp(data);
            newOption.OnSelect += OnSideSelected;
        }
        
        confirmButton.onClick.AddListener(Confirm);
    }

    private void SetUp(SideData data)
    {
        currentSide = data;
        baseOption.SetUp(currentSide);
        pipValue.Value = data.pips;
    }

    private void ChangePips(int value)
    {
        if (currentSide.pips < 0) return;
        OnSideSelected(currentSide);
    }

    private void OnSideSelected(SideData data)
    {
        currentSide = data;
        baseOption.SetUp(data, pipValue.Value);
    }

    private void Confirm()
    {
        SideData finalPick = currentSide;
        finalPick.pips = finalPick.pips < 0 ? -1 : pipValue.Value;
        onPickCallback?.Invoke(finalPick);
        onPickCallback = null;
        gameObject.SetActive(false);
    }
    
    public static void PickSide(SideData data, Action<SideData> onPick)
    {
        instance.gameObject.SetActive(true);
        instance.SetUp(data);
        instance.onPickCallback = onPick;
    }
}
