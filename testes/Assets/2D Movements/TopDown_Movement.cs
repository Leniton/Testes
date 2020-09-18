using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDown_Movement : MonoBehaviour
{
	/*aceleration/deceleration
	 */
    [SerializeField] int Velocidade;
	[SerializeField] [Tooltip("Quanto leva para chegar em velocidade máxima (1 = sem aceleração)")] [Range(0,1)] float Aceleração = 1;
	[SerializeField] [Tooltip("Quanto leva para parar (1 = sem desaceleração)")] [Range(0, 1)] float Desaceleração = 1;

	float acelerando, desacelerando;
	int vertical, horizontal;
	Vector2 AfflictedForce;


	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			vertical = -1;
		}
		else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			vertical = 0;
		}
		
	}

	private void FixedUpdate()
	{
		Move(new Vector3(horizontal, vertical, 0));
		/*if(GetComponent<Rigidbody2D>().velocity == Vector2.zero &&
		   (vertical == 0 && horizontal == 0))
		{
			acelerando = 0;
			desacelerando = 0;
			print("parou");
		}*/
	}

	Vector2 AceDesce(Vector2 v)
	{
		if(Aceleração == 1 && Desaceleração == 1) return v;

		Vector2 vel = GetComponent<Rigidbody2D>().velocity;
		float newX = 0;
		float newY = 0;

		if(Mathf.Abs(vel.x) > Mathf.Abs(v.x) || (Mathf.Abs(vel.y) > Mathf.Abs(v.y)))
		{
			desacelerando += Desaceleração;
			if (desacelerando > 1) desacelerando = 1;
		}
		if (Mathf.Abs(vel.x) < Mathf.Abs(v.x) || (Mathf.Abs(vel.y) < Mathf.Abs(v.y)))
		{
			acelerando += Aceleração;
			if (acelerando > 1) acelerando = 1;
		}

		if (Mathf.Abs(vel.x) > Mathf.Abs(v.x))
		{
			if (v.x == 0)
			{
				newX = vel.x * (1 - desacelerando) ;
			}
			else newX = v.x * desacelerando;
		}
		else if(Mathf.Abs(vel.x) < Mathf.Abs(v.x))
		{
			newX = v.x * acelerando;
		}

		v.x = newX;

		/*if (GetComponent<Rigidbody2D>().velocity.sqrMagnitude < v.sqrMagnitude)
		{
			acelerando += Aceleração;
			desacelerando = 0;
			if (acelerando > 1) acelerando = 1;

			v *= acelerando;
		}
		else
		{
			desacelerando += Desaceleração;
			acelerando = 0;
			if (desacelerando > 1) desacelerando = 1;

			v *= desacelerando;
		}*/

		return v;
	}

    public void Move(Vector2 dir)
    {
		dir *= Velocidade;
		//dir = AceDesce(dir);
		//print(dir.x);
		GetComponent<Rigidbody2D>().velocity = dir + AfflictedForce;
    }

	//when you can control the characterwhile moving
	public IEnumerator AfflictedMovement(Vector2 dir, float duracao)
	{
		AfflictedForce += dir;
		yield return new WaitForSeconds(duracao);

		AfflictedForce -= dir;
	}

	//when you can't control the characterwhile moving
	public void UncontrolableMovement(Vector2 dir, float duracao)
	{

	}
}
