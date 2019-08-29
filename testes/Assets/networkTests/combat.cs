using UnityEngine;
using UnityEngine.Networking;

public class combat : NetworkBehaviour {

	public const int Maxhealth = 100;
	public bool DestroyOnDeath;

	[SyncVar]
	public int health = Maxhealth;

	public void TakeDamage(int amount){

		if (!isServer) {
			return;
		}

		health -= amount;

		if (health <= 0) {
			if (DestroyOnDeath) {
				Destroy (gameObject);
			} else {
				health = Maxhealth;
				RpcRespawn ();
			}
		}
	}

	[ClientRpc]
	void RpcRespawn(){
		if (isLocalPlayer) {
			transform.position = Vector3.zero;
		}
	}
}
