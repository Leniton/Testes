using InputSystemHelper;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = InputSystemHelper.Input;
using Logger = LenixSO.Logger.Logger;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Plataform_Script plataform;
    
    private void Awake()
    {
        plataform ??= GetComponent<Plataform_Script>();
        if (plataform == null) return;
        Input.Map("Player").Action("Move").performed += OnMove;
        Input.Map("Player").Action("Move").canceled += OnMove;
        Input.Map("Player").Action("Jump").canceled += OnSwitch;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        var data = obj.ReadValue<Vector2>();
        plataform.input = data;
    }
    
    private void OnSwitch(InputAction.CallbackContext obj)
    {
        
    }
}
