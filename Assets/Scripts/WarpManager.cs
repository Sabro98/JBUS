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
    bool IsWarp;

    private void Awake()
    {
        IsWarp = false;
    }

    public void Warp(GameObject player, string to)
    {
        DontDestroyOnLoad(this.gameObject);
        IsWarp = true;
        var playerInfo = player.GetComponent<PlayerInfo>();
        warpPlayer = playerInfo.Player;
        this.to = to;
        SceneManager.LoadScene(JustLoadingScript.SceneName);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        if (!IsWarp) return;
        PhotonNetwork.JoinRoom(ROOM_NAME);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (!IsWarp) return;
        PhotonNetwork.CreateRoom(ROOM_NAME);
    }

    public override void OnJoinedRoom()
    {
        if (!IsWarp) return;
        PhotonNetwork.LoadLevel(to);
    }
}
