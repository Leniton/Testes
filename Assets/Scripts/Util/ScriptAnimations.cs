using System;
using System.Collections;
using UnityEngine;

namespace Util.Extensions
{
    public static class ScriptAnimations
    {
        public static IEnumerator Animate(Action<float> onUpdate, AnimationCurve curve = null, float? customDuration = null, bool invert = false)
        {
            curve ??= AnimationCurve.Linear(0, 0, 1, 1);
            float duration = curve.keys[^1].time;
            float timeScale = Mathf.Max(customDuration ?? duration, .0001f) / duration;
            float finalDuration = duration * timeScale;
            float time = curve.keys[0].time;
            
            //parameters used to invert
            float scaleAnchor = invert ? 1.0f : 0.0f;
            float scaleMultiplier = invert ? 1.0f : -1.0f;
            
            while (time < finalDuration)
            {
                float scaledTime = ((duration * scaleAnchor) - (time * scaleMultiplier)) / timeScale;
                onUpdate?.Invoke(curve.Evaluate(scaledTime));
                yield return null;
                time += Time.deltaTime;
            }
            onUpdate?.Invoke(curve.Evaluate((duration * scaleAnchor) - (1 * scaleMultiplier)));
        }

        public static IEnumerator MoveTo(this RectTransform rectTransform, Vector2 goal, AnimationCurve curve = null,
            float? customDuration = null)
        {
            Vector2 startingPosition = rectTransform.anchoredPosition;
            yield return Animate(t => rectTransform.anchoredPosition = Vector2.Lerp(startingPosition, goal, t), curve,
                customDuration);
        }

        public static IEnumerator MoveTo(this Transform transform, Vector2 goal, AnimationCurve curve = null,
            float? customDuration = null)
        {
            Vector2 startingPosition = transform.localPosition;
            yield return Animate(t => transform.localPosition = Vector2.Lerp(startingPosition, goal, t), curve,
                customDuration);
        }
    }
}
