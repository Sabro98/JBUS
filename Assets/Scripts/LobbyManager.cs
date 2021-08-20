using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField IDInput;

    public string playerID;

    const string ROOM_NAME = "JBNU";
    const string GAME_SENCE = "Game";
    const string JOIN_SCENE = "Join";

    private void Awake()
    {
        playerID = "";
    }

    private void Start()
    {

    }

    public void join()
    {
        SceneManager.LoadScene(JOIN_SCENE);
    }

    public void Login()
    {
        playerID = IDInput.text;
        if (string.IsNullOrEmpty(playerID)) return;

        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.JoinRoom(ROOM_NAME);    
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(ROOM_NAME);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(GAME_SENCE);
    }
}
