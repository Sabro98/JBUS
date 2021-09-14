using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ChangeChannel : MonoBehaviourPunCallbacks
{
    string ChangeTargetRoomName;
    bool IsChangeChannel;

    private void Awake()
    {
        ChangeTargetRoomName = "";
        IsChangeChannel = false;
    }

    public void Change(string to)
    {
        ChangeTargetRoomName = to;
        //DontDestroyOnLoad(this);
        //SceneManager.LoadScene(JustLoadingScript.SceneName);
        IsChangeChannel = true;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        if (!IsChangeChannel) return;
        PhotonNetwork.JoinRoom(ChangeTargetRoomName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (!IsChangeChannel) return;
        PhotonNetwork.CreateRoom(ChangeTargetRoomName);
    }

    public override void OnJoinedRoom()
    {
        if (!IsChangeChannel) return;
        PhotonNetwork.LoadLevel("Game");
        //TODO: GameMananger에서 유저의 정보를 못가져옴
        //Destroy(this);
    }
}
