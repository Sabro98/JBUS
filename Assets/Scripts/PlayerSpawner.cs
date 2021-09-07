using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public float minx, maxx, minz, maxz;

    public void Spawn(JBUS_Player currPlayer)
    {
        Vector3 randomPosition = new Vector3(Random.Range(minx, maxx), 5, Random.Range(minz, maxz)); //set random position
        //string playerModel = currPlayer.playerModel;
        var player = PhotonNetwork.Instantiate(currPlayer.playerModel, randomPosition, Quaternion.identity);    //Instantiate player use PhotonNetwork

        //init playerInfo
        var playerInfo = player.GetComponent<PlayerInfo>();
        playerInfo.Player = currPlayer;
    }
}
