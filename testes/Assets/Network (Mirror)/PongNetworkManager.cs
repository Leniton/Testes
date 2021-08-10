using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PongNetworkManager : NetworkManager
{
    public List<Transform> Spawns = new List<Transform>();

    public GameObject Ball;

    public List<PongPlayer> playerList = new List<PongPlayer>();

    public override void OnServerAddPlayer(NetworkConnection con)
    {
        Transform spawnPoint = Spawns[numPlayers];
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        player.GetComponent<PongPlayer>().texto = FindObjectOfType<Canvas>().transform.GetChild(numPlayers).GetComponent<UnityEngine.UI.Text>();
        playerList.Add(player.GetComponent<PongPlayer>());

        NetworkServer.AddPlayerForConnection(con, player);

        if (numPlayers == 2)
        {
            Ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            Ball.GetComponent<PongBall>().networkManager = this;
            NetworkServer.Spawn(Ball);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (Ball != null)
            NetworkServer.Destroy(Ball);

        playerList.Clear();

        base.OnServerDisconnect(conn);
    }
}
