using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_element : MonoBehaviour {

	public Material[] magia;
	public ParticleSystem[] particulas;

	public void MakeElement(string Element, GameObject localCast){
		GameObject form;
		form = localCast.transform.GetChild(0).gameObject;
		switch (Element) {
		case "Fogoe:":
			FireElement (form);
			break;
		case"Geloe:":
			IceElement (form);
			break;
		case"Raioe:":
			RaioElement (form);
			break;
		}
	}

	void FireElement(GameObject form){
		form.GetComponent<MeshRenderer>().material.color = Color.red;
	}

	void IceElement(GameObject form){
		form.GetComponent<MeshRenderer>().material.color = Color.cyan;
	}

	void RaioElement(GameObject form){
		form.GetComponent<MeshRenderer>().material.color = Color.yellow;
	}
}
