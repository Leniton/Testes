using System;
using Unity.Cinemachine;
using UnityEngine;
using Util.Extensions;

public class EjectionScript : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CinemachineCamera cam;

    private float defaultSize;
    private readonly float changeSize = 9;

    private void Awake()
    {
        defaultSize = cam.Lens.OrthographicSize;
        playerInput.onEjectSetup += EjectionSetup;
        playerInput.onEjectBegin += EjectionBegin;
        playerInput.onEjectBegin += EjectionEnd;
    }

    private void EjectionSetup()
    {
        StartCoroutine(ScriptAnimations.Animate(f =>
            cam.Lens.OrthographicSize = Mathf.Lerp(defaultSize, changeSize, f), customDuration: .2f));
    }

    private void EjectionBegin()
    {
        StartCoroutine(ScriptAnimations.Animate(f =>
            cam.Lens.OrthographicSize = Mathf.Lerp(changeSize, defaultSize, f), customDuration: .2f));
    }

    private void EjectionEnd()
    {
        
    }
}
