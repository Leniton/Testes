using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystemHelper
{
    public static class Input
    {
        public static InputActionMap Map(string map) => InputSystem.actions.FindActionMap(map);
        public static InputAction Action(this InputActionMap map, string action) => map.FindAction(action);
        public static InputAction Action(string action) => InputSystem.actions.FindAction(action);
    }
}