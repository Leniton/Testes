using System;
using UnityEngine;
using UnityEngine.UI;

public class Showcase : MonoBehaviour
{
    [SerializeField] private Button respawnButton;
    [SerializeField] private GameObject destructiblePrefab;
    
    private GameObject destructible;

    private void Awake()
    {
        respawnButton.onClick.AddListener(RespawnObject);
    }
    
    private void RespawnObject()
    {
        if (!ReferenceEquals(destructible, null)) return;
        destructible = Instantiate(destructiblePrefab);
    }
}
