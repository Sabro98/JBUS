using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        print("Try Connect to Master");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        print("Connected To Master!");
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Join");
        print("Joined at Lobby!");
    }
}
