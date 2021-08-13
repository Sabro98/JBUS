using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField IDInput;

    public string playerID;

    const string ROOM_NAME = "JBNU";
    const string LOGIN_SCENE = "EnterToGame";

    private void Awake()
    {
        playerID = "";
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void JoinRoom()
    {
        playerID = IDInput.text;
        if (string.IsNullOrEmpty(playerID)) return;

        PhotonNetwork.JoinRoom(ROOM_NAME);    
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(ROOM_NAME);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(LOGIN_SCENE);
    }
}
