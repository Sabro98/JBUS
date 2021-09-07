using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

//마스터서버와 연결해주는 역할
public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        //print("Try Connect to Master");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        //print("Connected To Master!");
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
        //print("Joined at Lobby!");
    }
}
