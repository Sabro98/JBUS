using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    const string LOBBY_OBJECT_NAME = "LobbyManager";

    [SerializeField] List<GameObject> prefabs;

    LobbyManager lobbyScript;

    public float spawnX;
    public float spawnZ;

    void Start()
    {
        //init playerId
        var lobbyObject = GameObject.Find(LOBBY_OBJECT_NAME);
        lobbyScript = lobbyObject.GetComponent<LobbyManager>();
        var LoginPlayer = lobbyScript.LoginPlayer;
        string playerNickName = LoginPlayer.playerNickName;
        string playerModel = "P" + LoginPlayer.playerModel;
        Destroy(lobbyObject.gameObject);
        
        SpawnPlayer(playerNickName, playerModel);
    }

    void SpawnPlayer(string playerNickName, string playerModel)
    {
        var prefab = getPlayerPrefab(playerModel);
        Vector3 randomPosition = new Vector3(Random.Range(-spawnX, spawnX), 5, Random.Range(-spawnZ, spawnZ));
        var player = PhotonNetwork.Instantiate(prefab.name, randomPosition, Quaternion.identity);
        player.GetComponent<PlayerInfo>().SetPlayerID(playerNickName);
    }

    GameObject getPlayerPrefab(string prefabName)
    {
        GameObject returnObj = null;

        foreach(var prefab in prefabs)
        {
            if (prefab.name.Equals(prefabName))
            {
                returnObj = prefab;
                break;
            }
        }

        return returnObj;
    }
}
