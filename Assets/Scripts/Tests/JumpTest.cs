using PhysicsHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour
{
    private Plataform_Script plataform;

    //test parameters
    bool pause = false;
    bool checkStopTime = false;
    private float stopTime;
    private float speedLost;

    private void Awake()
    {
        plataform = GetComponent<Plataform_Script>();

        plataform.Jump.OnLand += CollisionEnter;
    }

    private void Update()
    {
        checkStopTime = plataform.state == Plataform_Script.State.jumping;
    }

    private void FixedUpdate()
    {
        stopTime += Time.fixedDeltaTime;
        float gravityForce = plataform.Jump.GravityForce();
        speedLost += gravityForce;
        if (checkStopTime)
        {
            speedLost += gravityForce;
        }
        if (gravityForce <= 0)
        {
            if (!checkStopTime)
            {
                checkStopTime = true;
                Debug.LogError($"stop time: {stopTime} | height: {plataform.transform.position.y} \nspeed lost: {speedLost}");
                //if(pause) UnityEditor.EditorApplication.isPaused = true;
                //Debug.DrawRay(transform.position, Vector3.down * t_initialHeight, Color.red + (Color.yellow / 2), .2f);
                stopTime = 0;
                speedLost = 0;
            }
        }
    }

    private void CollisionEnter(CollisionData data)
    {
        //test only
        //Debug.LogError($"it took {stopTime}s to land, at {data.relativeVelocity} speed");
        pause = false;
        checkStopTime = false;
        stopTime = 0;
        speedLost = 0;
    }
}
