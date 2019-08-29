using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combo_animation_scr : MonoBehaviour {

	Animator controler;

	void Start(){
		controler = GetComponentInChildren<Animator> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Z)) {
			controler.SetTrigger ("PunchTrigger");
		}
	}
}
