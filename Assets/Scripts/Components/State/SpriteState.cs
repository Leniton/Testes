using UnityEngine;
using UnityEngine.UI;

public class SpriteState : DualState
{
    public Sprite onState, offState;
    [SerializeField] Image image;

    public override void SetState(bool state)
    {
        image.sprite = state ? onState : offState;
    }
}
