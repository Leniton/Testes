using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public abstract class LenixSOLayoutGroup : MonoBehaviour
{
    [Tooltip("If true this layout group will update its positions every frame")] public bool isStatic = false;
    public float spacing = 50; 
    public float offset;

    public List<RectTransform> overrideElements = new();

    protected virtual void OnEnable() => AdjustElements();

    protected virtual void Update()
    {
        if (isStatic && Application.isPlaying) return; //Check for application playing to ignore it when in editor
        AdjustElements();
    }

    public RectTransform[] GetEnabledElements()
    {
        if (overrideElements is { Count: <= 0 })
        {
            List<RectTransform> enabledChilds = new(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform rectTransform = (RectTransform)transform.GetChild(i);
                var layoutElement = rectTransform.GetComponent<LayoutElement>();
                if (layoutElement is { ignoreLayout: true } || 
                    !rectTransform.gameObject.activeSelf) continue;
                enabledChilds.Add(rectTransform);
            }
            return enabledChilds.ToArray();
        }
        return overrideElements.ToArray();
    }

    public abstract void AdjustElements();
}
