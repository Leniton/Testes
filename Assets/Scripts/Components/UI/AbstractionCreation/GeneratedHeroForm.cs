using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneratedHeroForm : MonoBehaviour
{
    [SerializeField] TMP_InputField heroTier;
    [SerializeField] TMP_Dropdown heroColor;
    [Space,SerializeField] API_Manager manager;

    public void Create()
    {
        manager.CreateAbstraction(WindowGenerator.Hero_Generated((HeroColor)heroColor.value, int.Parse(heroTier.text)));
    }
}
