using UnityEngine;

public class SideOptionWindow : MonoBehaviour
{
    [SerializeField] private SideOption baseOption;
    [SerializeField] private RectTransform content;
    [SerializeField] private ValueInputField pipValue;
    private SideData currentSide;
    
    private void Awake()
    {
        pipValue.onValueChanged += ChangePips;
        for (int i = 0; i < DiceSideDatabase.sides.Length; i++)
        {
            SideData data = DiceSideDatabase.sidesData[i];

            SideOption newOption = Instantiate(baseOption, content);
            newOption.gameObject.SetActive(true);
            newOption.SetUp(data);
            newOption.OnSelect += OnSideSelected;
        }
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
}
