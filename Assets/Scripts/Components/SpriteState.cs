using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(StateButton))]
public class SpriteState : MonoBehaviour
{
    public Sprite onState, offState;
    [SerializeField] Image image;

    StateButton stateButton;

    private void Awake()
    {
        stateButton = GetComponent<StateButton>();
        stateButton.onStateChange.AddListener(SetState);
        SetState(stateButton.State);
    }

    public void SetState(bool state)
    {
        image.sprite = state ? onState : offState;
    }
}
