using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cast : MonoBehaviour {

	public string spell;
	public GameObject canvas;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.M)){
			canvas.SetActive(!canvas.activeSelf);
		}
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			GetComponent<Chanting_scr> ().Words (spell);
		}
	}
}
