using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Movement : Displacement
{
    //movement parameters
    [SerializeField, Min(0)] float timeToTopSpeed, topSpeed, timeToStop;
    private float accelerationRate, decelerationRate;
    private float lastUpdateTime = -1;
    private float currentTime = 0;
    private Vector3 lastDirection;
    enum CollisionRule { block, slide }
    [SerializeField] CollisionRule collisionRule;
    private List<CollisionData> lastCollision = new();

    public override void Init(PhysicsHandler handler)
    {
        base.Init(handler);
        physicsHandler.CollisionEnter += CollisionEnter;
        physicsHandler.CollisionExit += CollisionExit;
    }

    public override void CalculateParameters()
    {
        accelerationRate = timeToTopSpeed > 0 ? 1f / timeToTopSpeed : 999999999f;
        decelerationRate = timeToStop > 0 ? 1f / timeToStop : 999999999f;
    }

    public Vector3 Move(Vector3 direction)
    {
        if (lastUpdateTime < 0) lastUpdateTime = Time.time;
        float elapsedTime = Time.time - lastUpdateTime;
        float modifier;

        if (direction == Vector3.zero)
        {
            modifier = -decelerationRate;
            direction = lastDirection;
        }
        else
        {
            modifier = accelerationRate;
            lastDirection = direction;
        }

        for (int i = 0; i < lastCollision.Count; i++)
        {
            if (Vector3.Angle(-lastCollision[i].contacts[0].normal, direction) < 90)
            {
                if (collisionRule == CollisionRule.slide)
                {
                    direction = AdjustToNormal(direction, lastCollision[i].contacts[0].normal);
                }
                else
                {
                    direction = Vector3.zero;
                }
                break;
            }
        }

        

        modifier *= elapsedTime;
        currentTime = Mathf.Clamp01(currentTime + modifier);
        float ajustedSpeed = Mathf.Lerp(0, topSpeed, currentTime);
        direction *= ajustedSpeed;

        lastUpdateTime = Time.time;
        if (currentTime <= 0 && modifier < 0) lastUpdateTime = -1;

        return direction;
    }

    //to block movement against a wall, block movement in which the angle against -normal is < 90º ?

    public Vector3 AdjustToNormal(Vector3 input, Vector3 normal)
    {
        if (input.x == 0 && input.z == 0) return Vector3.zero;
        Vector3 direction = new Vector3(input.x, 0, input.z);

        Vector3.OrthoNormalize(ref normal, ref direction);
        return direction;
    }

    void CollisionEnter(CollisionData data)
    {
        for (int i = 0; i < lastCollision.Count; i++)
        {
            if (data.gameObject == lastCollision[i].gameObject)
            {
                break;
            }
        }
        lastCollision.Add(data);
    }
    void CollisionExit(CollisionData data)
    {
        for (int i = 0; i < lastCollision.Count; i++)
        {
            if(data.gameObject == lastCollision[i].gameObject)
            {
                lastCollision.RemoveAt(i);
                break;
            }
        }
    }
}
