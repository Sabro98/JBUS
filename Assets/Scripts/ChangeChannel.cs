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

    public void Change(GameObject player, string to)
    {
        ChangeTargetRoomName = to;
        //DontDestroyOnLoad(this);
        //SceneManager.LoadScene(JustLoadingScript.SceneName);
        IsChangeChannel = true;
        var playerObj = Instantiate(Resources.Load("PlayerInfoObj")) as GameObject;
        playerObj.GetComponent<ForPlayerSpawn>().SpawnPlayer = player.GetComponent<PlayerInfo>().Player.Clone() as JBUS_Player;
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
        //Destroy(this);
    }
}

//TODO: ChangeChannel 할 때 latestChannel 갱신 x
//이걸 어떻게 해결? WarpManager와 ChangeChannel 둘 중 하나만 쓰는게 좋아보임.