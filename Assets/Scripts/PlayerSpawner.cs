using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public float spawnX;
    public float spawnZ;

    public void Spawn(string playerNickName, string playerModel)
    {
        Vector3 randomPosition = new Vector3(Random.Range(-spawnX, spawnX), 5, Random.Range(-spawnZ, spawnZ));
        var player = PhotonNetwork.Instantiate(playerModel, randomPosition, Quaternion.identity);
        player.GetComponent<PlayerInfo>().SetPlayerID(playerNickName);
    }
}
