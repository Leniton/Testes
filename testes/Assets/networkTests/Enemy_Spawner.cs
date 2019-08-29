using UnityEngine;
using UnityEngine.Networking;

public class Enemy_Spawner : NetworkBehaviour {

	public GameObject enemy_pref;
	public int nEnemies;

	public override void OnStartServer ()
	{
		for (int i = 0; i < nEnemies; i++) {
			Vector3 pos = new Vector3 (Random.Range (-30f, 30f), Random.Range (-20f, 20f),0);

			Quaternion rotation = Quaternion.Euler (0, 0, Random.Range (0, 180));

			GameObject enemy = Instantiate (enemy_pref, pos, rotation);
			NetworkServer.Spawn (enemy);
		}
	}
}
