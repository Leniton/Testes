using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_look : MonoBehaviour {

	Vector3 angle;
	[Range(1f,4f)]
	public float sen = 1f;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update () 
	{

		angle = new Vector3 (-Input.GetAxis ("Mouse Y")*sen + transform.eulerAngles.x, Input.GetAxis ("Mouse X")*sen + transform.eulerAngles.y, 0);
		transform.eulerAngles = (angle);

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (Cursor.lockState == CursorLockMode.Locked) 
			{
				Cursor.lockState = CursorLockMode.None;
			} else if (Cursor.lockState == CursorLockMode.None) 
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		transform.eulerAngles = (angle);
		if(Input.GetKeyDown(KeyCode.Q)){
			sen += 0.5f;
		}
		if(Input.GetKeyDown(KeyCode.W)){
			sen -= 0.5f;
		}
	}
}
