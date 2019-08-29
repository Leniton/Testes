using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoCodigo : MonoBehaviour {

	public bool Editable;
	[SerializeField]
	GameObject codeScreen;
	public List<string> codigos;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown()
	{
		StartCoroutine("AtivarInterface");
	}

	IEnumerator AtivarInterface()
	{
		if (!codeScreen.activeSelf)
		{
			codeScreen.SetActive(true);
			yield return new WaitForSeconds(Time.deltaTime);
			codeScreen.GetComponentInChildren<TypeCode>().IniciarInterface(this);
		}
		else
		{
			yield return null;
		}
	}
}
