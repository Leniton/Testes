using UnityEngine;
using UnityEngine.Networking;

public class playerMov : NetworkBehaviour {

	float x,y;

	public GameObject bullet;
	
	public override void OnStartLocalPlayer(){
		GetComponent<SpriteRenderer> ().color = Color.blue;
	}

	void Update () {

		if (!isLocalPlayer) {
			return;
		}
		x = Input.GetAxis ("Horizontal") * 0.1f;
		y = Input.GetAxis ("Vertical") * 0.1f;

		transform.Translate (x, y, 0);

		if (Input.GetKeyDown (KeyCode.Space)) {
			Cmdshoot ();
		}
	}

	[Command]
	void Cmdshoot(){
		GameObject inst_bullet = Instantiate (bullet, transform.position + transform.up*3f, Quaternion.identity);

		inst_bullet.GetComponent<Rigidbody2D> ().velocity = transform.up * 25;

		NetworkServer.Spawn (inst_bullet);

		Destroy (inst_bullet, 2.0f);
	}

}
