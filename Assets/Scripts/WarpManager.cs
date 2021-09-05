using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class WarpManager : MonoBehaviour
{
    public JBUS_Player warpPlayer { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Warp(GameObject player, string to)
    {
        var playerInfo = player.GetComponent<PlayerInfo>();
        warpPlayer = new JBUS_Player(playerInfo.PlayerName, playerInfo.PlayerModel, playerInfo.PlayerID);
        //PhotonNetwork.LoadLevel(to);
        SceneManager.LoadScene(to);
    }
}
