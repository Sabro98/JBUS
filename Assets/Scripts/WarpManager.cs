using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class WarpManager : MonoBehaviourPunCallbacks
{
    public JBUS_Player warpPlayer { get; set; }
    const string ROOM_NAME = "TEST";
    string to;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Warp(GameObject player, string to)
    {
        var playerInfo = player.GetComponent<PlayerInfo>();
        warpPlayer = playerInfo.Player;
        this.to = to;
        SceneManager.LoadScene("JustLoadingTitle");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRoom(ROOM_NAME);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(ROOM_NAME);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(to);
    }
}
