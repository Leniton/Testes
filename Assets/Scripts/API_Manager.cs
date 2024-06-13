using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class API_Manager : MonoBehaviour
{
    [SerializeField] private RectTransform MainParent;
    [SerializeField] CreationMenu CreationMenu;
    [SerializeField] Button copyButton;
    private MonoAbstraction currentCreation;
    
    private void Awake()
    {
        CreationMenu.onCreated += CreateAbstraction;
        PickCreation();
        copyButton.onClick.AddListener(CopyCode);
    }

    public void PickCreation()
    {
        if (currentCreation != null) Destroy(currentCreation.gameObject);
        CreationMenu.OpenOptions();
    }

    public void CreateAbstraction(MonoAbstraction abstraction)
    {
        currentCreation = abstraction;
        abstraction.transform.SetParent(MainParent, false);
        abstraction.transform.SetAsFirstSibling();
    }

    public void CopyCode()
    {
        NoTask.WebGLSupport.Clipboard.ClipboardWebGLUtility.CopyTextToClipboard(currentCreation.GetCode(null));
    }
}
