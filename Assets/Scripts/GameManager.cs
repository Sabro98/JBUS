using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    const string JOIN_OBJECT_NAME = "JoinController";

    [SerializeField] List<GameObject> prefabs;

    JoinController joinScript;

    public float spawnX;
    public float spawnZ;

    void Start()
    {
        //init playerId
        var joinObject = GameObject.Find(JOIN_OBJECT_NAME);
        joinScript = joinObject.GetComponent<JoinController>();
        string playerName = joinScript.PlayerID;
        string prefabName = "P" + joinScript.CurrentPrefabName;
        Destroy(joinObject.gameObject);
        
        SpawnPlayer(playerName, prefabName);
    }

    //랜덤하게 캐릭터 생성 -> 데이터 베이스로 업그레이드 해야하나?
    void SpawnPlayer(string playerID, string prefabName)
    {
        //외형을 복사
        //var basic_rig = playerApperancePrefab.GetComponentInChildren<Transform>().Find("basic_rig").gameObject;
        //var common_people = playerApperancePrefab.GetComponentInChildren<Transform>().Find("common_people").gameObject;
        //basic_rig.transform.parent = playerPrefab.transform;
        //common_people.transform.parent = playerPrefab.transform;

        var prefab = getPlayerPrefab(prefabName);
        Vector3 randomPosition = new Vector3(Random.Range(-spawnX, spawnX), 5, Random.Range(-spawnZ, spawnZ));
        var player = PhotonNetwork.Instantiate(prefab.name, randomPosition, Quaternion.identity);
        player.GetComponent<PlayerInfo>().SetPlayerID(playerID);
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
