using UnityEngine;

public class Bullet : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D coll)
	{
		GameObject hit = coll.gameObject;
		combat hitCombat = hit.GetComponent<combat> ();

		if (hitCombat != null) {
			
			hitCombat.TakeDamage (10);
			Destroy (gameObject);
		}
	}
}
