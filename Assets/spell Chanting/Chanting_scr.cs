using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chanting_scr : MonoBehaviour {

	public Spell_form forma;
	public Spell_atributes atrubuto;
	public Spell_element elemento;

	public int manaCost;
	string kindOfSp = "fea";
	int point = 0;

	public GameObject[] points;

	public void Cast(){
		for (int i = 0; i <= point; i++) {
			//print (points [i]);
			if (points [point].transform.GetChild (0) != null) {
				points [point].GetComponentInChildren<Spell_Status> ().lançar ();
				points [point].transform.GetChild(0).SetParent(transform);
			}
		}
		point++;
		if (point <= points.Length) {
			point = 0;
		}
	}

	public void Words(string spell)
	{
		string word = "";
		for (int i = 0; i < spell.Length; i++) 
		{
			word += spell [i];
			if (spell [i] == ':') {
				if (spell [i-1] == kindOfSp [0]) {
					forma.Makeform (word, points [point]);
				}
				if (spell [i-1] == kindOfSp [1]) {
					elemento.MakeElement (word, points [point]);
				}
				word = "";
			}
		}
		Cast ();
	}
}
