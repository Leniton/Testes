using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Status : MonoBehaviour {

	public float dano;
	public float velocidade;

	Rigidbody rb;

	void Start () {
		
	}
	
	public void lançar (){
		rb = GetComponent<Rigidbody> ();
		rb.velocity = transform.forward * velocidade;
		Destroy (gameObject, 2);
	}
}
