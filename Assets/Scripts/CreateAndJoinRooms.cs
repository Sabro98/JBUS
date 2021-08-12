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
    [SerializeField] TMP_InputField createInput;
    [SerializeField] TMP_InputField joinInput;

    const string ROOM_NAME = "JBNU";
    const string GAME_SCENE = "GAME";

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(ROOM_NAME);    
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(ROOM_NAME);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(GAME_SCENE);
    }
}
