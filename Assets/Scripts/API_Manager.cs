
using UnityEngine;

public class API_Manager : MonoBehaviour
{
    [SerializeField] private RectTransform MainParent;
    
    private void Awake()
    {
        //Debug.Log(DiceSideDatabase.sides[1].name);
        MonoAbstraction item = WindowGenerator.Hero_Reference();
        item.transform.SetParent(MainParent, false);
    }
}
