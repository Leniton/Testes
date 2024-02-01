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
    float currentTime = 0;

    Vector3 initialPos;

    void Start()
    {
        initialPos = movingObject.transform.position;
    }

    void Update()
    {
        if (currentTime <= 1)
        {
            currentTime += Time.deltaTime * timeModifier;
            Vector3 value = Vector3.zero;
            value.x = (-initialPos.x*2) * curve.Evaluate(currentTime);
            movingObject.position = initialPos + value;
        }
        else
        {
            currentTime = 0;
        }
    }
}