using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StateButton : MonoBehaviour
{
    private bool state;
    private Button button;

    public bool State
    {
        get => state;
        set => SetState(value, true);
    }

    public UnityEvent<bool> onStateChange;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SwitchState);
    }

    public void SwitchState()
    {
        SetState(!state, true);
    }

    public void SetState(bool newState, bool notify = false)
    {
        state = newState;
        if(notify) onStateChange?.Invoke(state);
    }
}