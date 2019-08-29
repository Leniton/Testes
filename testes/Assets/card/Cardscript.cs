using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Cardscript : ScriptableObject {

	public int life;
	public string name;

	public void atack(){
		Debug.Log ("atk");
	}
}
