using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDisplayer : MonoBehaviour
{
    [SerializeField] Transform movingObject;
    [SerializeField] Procedural_Animator animator;
    [Header("Time values")]
    [SerializeField] float timeModifier = 1;
    [SerializeField] AnimationCurve curve;
    public float currentTime = 0;
    float currentInput = 0;

    Vector3 initialPos;
    List<InterpolationCurve> curves = new();
    List<InterpolationCurve> removeQueue = new();

    void Start()
    {
        initialPos = animator.StartingPosition;
        movingObject.position = initialPos;
        //curves.Add(new(curve));
    }

    void Update()
    {
        currentTime += Time.deltaTime * timeModifier;
        if (currentTime <= 1.1f)
        {
            float time = Mathf.Clamp01(currentTime);
            Vector3 value = Vector3.zero;
            value.x = animator.timeRange * time;

            if(currentInput != animator.GetInput(time))
            {
                curves.Add(new(curve));
                currentInput = animator.GetInput(time);
            }

            float finalValue = 0;
            for (int i = 0; i < curves.Count; i++)
            {
                finalValue += curves[i].currentValue;
                if (curves[i].ended)
                    removeQueue.Add(curves[i]);
            }
            Debug.Log(finalValue);
            value.y = (animator.inputChange * finalValue);

            movingObject.position = initialPos + value;

            /*for (int i = 0; i < removeQueue.Count; i++)
            {
                curves.Remove(removeQueue[0]);
                removeQueue.RemoveAt(0);
            }*/
        }
        else if (currentTime > 1.5f) 
        {
            currentTime = 0;
            currentInput = 0; 
            curves.Clear();
            removeQueue.Clear();
        }
    }
}