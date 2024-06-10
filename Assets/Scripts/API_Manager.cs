
using UnityEngine;

public class API_Manager : MonoBehaviour
{
    [SerializeField] private RectTransform MainParent;
    
    private void Awake()
    {
        MonoAbstraction item = WindowGenerator.Item_Generated();
        item.transform.SetParent(MainParent, false);
    }
}
