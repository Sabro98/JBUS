using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    const string LOGIN_OBJECT_NAME = "CreateAndJoinRooms";

    [SerializeField] GameObject playerPrefab;

    CreateAndJoinRooms lobbyScript;

    public float spawnX;
    public float spawnZ;

    void Start()
    {
        //init playerId
        var lobbyObject = GameObject.Find(LOGIN_OBJECT_NAME);
        lobbyScript = lobbyObject.GetComponent<CreateAndJoinRooms>();
        string playerName = lobbyScript.playerID;
        Destroy(lobbyObject.gameObject);
        
        SpawnPlayer(playerName);
    }

    void SpawnPlayer(string playerID)
    {
        Vector3 randomPosition = new Vector3(Random.Range(-spawnX, spawnX), 5, Random.Range(-spawnZ, spawnZ));
        var player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        player.GetComponent<PlayerInfo>().SetPlayerID(playerID);
    }
}
