using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_card : MonoBehaviour {

	public GameObject card;
	Vector3 pos;
	Ray2D ray;
	RaycastHit2D hit;

	void Update () {
		if (Input.GetMouseButtonDown (0)) 
		{
			ray = ChangeRay (Camera.main.ScreenPointToRay (Input.mousePosition));
			hit = Physics2D.Raycast (ray.origin, ray.direction);
			if (hit.collider != null && hit.collider.GetComponent<Card_Data>() != null) {
				card = hit.collider.gameObject;
			}
		}

		if (Input.GetMouseButton (0)) 
		{
			if (card != null) {
				pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10));
				card.transform.position = pos;
			}
		}

		if (Input.GetMouseButtonUp (0))
		{
			card.GetComponent<Card_Data>().data.life++;
			card = null;
			hit = new RaycastHit2D ();
		}
	}

	Ray2D ChangeRay(Ray ry){
		Ray2D Ret;
		Ret = new Ray2D(new Vector2(ry.origin.x,ry.origin.y),new Vector2(ry.direction.x,ry.direction.y));
		return Ret;
	}
}
