using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_form : MonoBehaviour {

	public GameObject magia;

	public void Makeform(string form, GameObject localCast){
		switch (form) {
		case "Balaf:":
			BulletForm (localCast);
			break;
		case"Esferaf:":
			EsphereForm (localCast);
			break;
		case"Círculof:":
			CirleForm (localCast);
			break;
		}
	}

	public void BulletForm(GameObject local){
		
		//magia = Instantiate (GameObject.CreatePrimitive (PrimitiveType.Sphere), GameObject.Find ("Spell").transform);
		magia = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		magia.transform.SetParent (local.transform);
		magia.transform.localEulerAngles = Vector3.zero;
		magia.transform.localPosition = new Vector3 (0, 0, 0);
		magia.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		magia.AddComponent<Spell_Status> ();
		magia.GetComponent<Spell_Status> ().dano = 2;
		magia.GetComponent<Spell_Status> ().velocidade = 10;

		magia.AddComponent<Rigidbody> ();
		magia.GetComponent<Rigidbody> ().useGravity = false;

	}

	public void EsphereForm(GameObject local){
		magia = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		magia.transform.SetParent (local.transform);
		magia.transform.localEulerAngles = Vector3.zero;
		magia.transform.localPosition = new Vector3 (0, 0, 0);
		magia.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		magia.AddComponent<Spell_Status> ();
		magia.GetComponent<Spell_Status> ().dano = 5;
		magia.GetComponent<Spell_Status> ().velocidade = 4;

		magia.AddComponent<Rigidbody> ();
		magia.GetComponent<Rigidbody> ().useGravity = false;
	}


	public void CirleForm(GameObject local){
		magia = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
		magia.transform.SetParent (local.transform);
		magia.transform.localEulerAngles = Vector3.zero;
		magia.transform.localPosition = new Vector3 (0, 0, 5);
		magia.transform.localScale = new Vector3 (3f, 0.01f, 3f);
	}
}