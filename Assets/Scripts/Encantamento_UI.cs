using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encantamento_UI : MonoBehaviour {

	public string encantamento;

	public GameObject[] Atr_barra = new GameObject[2];
	[Space]
	public GameObject Add_bar, tipo_Bar,objec;
	public Transform ViewportContent;

	public List<Dropdown> B_atributos;
	public List<int> Atributos;
	public List<int> Apr_escolhidos;

	public void Começo(){
		print("começo");
		Atributos.Clear ();
		B_atributos.Clear ();
		B_atributos.Insert(0,tipo_Bar.GetComponentInChildren<Dropdown>());
		B_atributos.Insert(1,GameObject.Find("ADD_spellOptions").GetComponentInChildren<Dropdown>());
		Atributos.Add (0);
		Apr_escolhidos.Add (0);
	}

	public void Tipo_opt(int tipo_Index){
		if (Atributos.ToArray ().Length > 0) {
			Atributos.RemoveAt (0);
		}
		Atributos.Insert (0, tipo_Index);
		//updateIndexCount ();
	}

	public void Add_Atributo(){
		//9 limite
		GameObject barra = Instantiate (Atr_barra[B_atributos[B_atributos.ToArray ().Length-1].value],ViewportContent);
		barra.name = "barra :" + (B_atributos.ToArray ().Length - 1);
		B_atributos.Insert(B_atributos.ToArray ().Length-1,barra.GetComponentInChildren<Dropdown>());
		Atributos.Add (0);
		Apr_escolhidos.Add (B_atributos [B_atributos.ToArray ().Length - 1].value+1);
		if (B_atributos.ToArray ().Length == 6) {
			RectTransform Add = B_atributos [B_atributos.ToArray ().Length - 1].GetComponent<RectTransform> ();
			Destroy (Add.parent.gameObject);
			B_atributos.RemoveAt (B_atributos.ToArray ().Length - 1);
		}
		Organize ();
	}

	public void RemoverAtributo(GameObject obj){
		for (int i = 1; i < B_atributos.ToArray ().Length; i++) 
		{
			if (B_atributos[i] == obj.GetComponentInChildren<Dropdown>()) 
			{
				B_atributos.RemoveAt (i);
				Atributos.RemoveAt (i);
				Apr_escolhidos.RemoveAt (i);
				Destroy (obj);
			}
		}
		if (B_atributos [B_atributos.ToArray ().Length - 1].GetComponent<RectTransform> ().parent.gameObject != GameObject.Find("ADD_spellOptions")) {
			GameObject Add_Atribure = Instantiate (Add_bar, ViewportContent);
			Add_Atribure.name = "ADD_spellOptions";
			B_atributos.Add (Add_Atribure.GetComponentInChildren<Dropdown> ());
		}
		Organize ();
	}

	public void Edit_atributo(int Atr_index){
		for (int i = 1; i < B_atributos.ToArray ().Length; i++) 
		{
			if (B_atributos[i].GetComponent<RectTransform> ().parent.gameObject == objec)
			{
				Atributos.RemoveAt (i);
				Atributos.Insert (i, Atr_index);
				break;
			}
		}
		objec = null;
	}

	void Organize (){
		for (int i = 1; i < B_atributos.ToArray ().Length; i++) {
			if (B_atributos [i] != null) {
				RectTransform pos = B_atributos[i].GetComponent<RectTransform>();
				/*print ("pai: " + pos.parent.GetComponent<RectTransform> ());
				print ("filho: " + pos);
				print ("nulo: " + pos.GetComponent<BoxCollider2D>());*/
				pos.parent.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (5, -40 - 30 * (i - 1));
			} else {
				print ("nulo");
				B_atributos.RemoveAt (i);
				Atributos.RemoveAt (i);
			}
		}
	}

	public void ConstruirEncantamento(){
		encantamento = "";
		for (int i = 0; i < B_atributos.ToArray ().Length; i++) {
			char keyChar = ' ';
			//print(B_atributos[i]);
			if (i != Apr_escolhidos.ToArray ().Length) {
				//print(Apr_escolhidos[i]);
				switch (Apr_escolhidos [i]) {
				case 0:
					keyChar = 'f';
					break;
				case 1:
					keyChar = 'e';
					break;
				case 2:
					keyChar = 'a';
					break;

				}
				encantamento += B_atributos [i].captionText.text + keyChar + ':';
				B_atributos.Remove(B_atributos[i]);
			}
		}
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Cast>().spell = encantamento;
	}

	public void getObject(GameObject instance){
		objec = instance;
	}
}
