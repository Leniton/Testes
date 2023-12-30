using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTester : MonoBehaviour
{
    PhysicsHandler physicsHandler;
    float prevCheckpointTime = 0;

    [SerializeField] float speedTrigger;

    private void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();
        physicsHandler.TriggerEnter += CheckSpeed;
        StartCoroutine(Test(speedTrigger));
    }

    private void CheckSpeed(ColliderData data)
    {
        Debug.Log($"hit at {physicsHandler.GetVelocity().x} speed on {Time.time - prevCheckpointTime} seconds");
        prevCheckpointTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StopAllCoroutines();
            StartCoroutine(Test(0));
        }
        if(Input.GetKeyUp(KeyCode.A))
        {
            StopAllCoroutines();
            StartCoroutine(Test(speedTrigger));
        }
    }

    IEnumerator Test(float goal)
    {
        float timing = 0;
        float currentSpeed = physicsHandler.GetVelocity().x;
        do
        {
            yield return new WaitForFixedUpdate();
            timing += Time.fixedDeltaTime;
            currentSpeed = physicsHandler.GetVelocity().x;

        } while(currentSpeed != goal);
        //Debug.Log($"reached speed in {timing} seconds");
    }
}
