using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    const string LOGIN_OBJECT_NAME = "LobbyManager";

    [SerializeField] GameObject playerPrefab1, playerPrefab2;

    LobbyManager lobbyScript;

    public float spawnX;
    public float spawnZ;

    void Start()
    {
        //init playerId
        var lobbyObject = GameObject.Find(LOGIN_OBJECT_NAME);
        lobbyScript = lobbyObject.GetComponent<LobbyManager>();
        string playerName = lobbyScript.playerID;
        Destroy(lobbyObject.gameObject);
        
        SpawnPlayer(playerName);
    }

    void SpawnPlayer(string playerID)
    {
        //외형을 복사
        //var basic_rig = playerApperancePrefab.GetComponentInChildren<Transform>().Find("basic_rig").gameObject;
        //var common_people = playerApperancePrefab.GetComponentInChildren<Transform>().Find("common_people").gameObject;
        //basic_rig.transform.parent = playerPrefab.transform;
        //common_people.transform.parent = playerPrefab.transform;

        //랜덤하게 캐릭터 생성 -> 데이터 베이스에서 캐릭터 정보 가져와야할L 
        int rand = Random.Range(0, 2);
        GameObject selectedPlayer = (rand % 2 == 1 ? playerPrefab1 : playerPrefab2);
        Vector3 randomPosition = new Vector3(Random.Range(-spawnX, spawnX), 5, Random.Range(-spawnZ, spawnZ));
        var player = PhotonNetwork.Instantiate(selectedPlayer.name, randomPosition, Quaternion.identity);
        player.GetComponent<PlayerInfo>().SetPlayerID(playerID);
    }
}
