using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{

	[SerializeField] Movement movement;
	[SerializeField] PhysicsHelper.PhysicsHandler PhysicsHandler;
	int vertical, horizontal;

    private void Awake()
    {
		movement.Initialize(PhysicsHandler);
    }

    // Update is called once per frame
    void Update()
	{
		Mover();
	}

	void Mover()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			horizontal = -1;
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			horizontal = 1;
		}
		else if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
		{
			horizontal = 0;
		}

		if (Input.GetKeyDown(KeyCode.W))
		{
			vertical = 1;
		}else if (Input.GetKeyDown(KeyCode.S))
		{
			vertical = -1;
		}else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			vertical = 0;
		}
		PhysicsHandler.Velocity = movement.Move(new Vector3(horizontal, vertical));
    }
}
