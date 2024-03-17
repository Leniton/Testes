using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverKeyword : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text word;
    [SerializeField] Keyword keyword;
    private void Awake()
    {
        word.text = keyword;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PopUpComponent.instance.Show(keyword.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PopUpComponent.instance.Hide();
    }
}
