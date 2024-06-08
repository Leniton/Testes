using UnityEngine;
using UnityEngine.UI;

public class SpriteState : MonoBehaviour
{
    public Sprite onState, offState;
    [SerializeField] Image image;

    public void SetState(bool state)
    {
        image.sprite = state ? onState : offState;
    }
}
