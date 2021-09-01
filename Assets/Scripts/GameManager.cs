using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject spawner;

    public GameObject WarpedPlayer;

    const string LOBBY_OBJECT_NAME = "LobbyManager";
    const string ROOM_NAME = "JBNU";

    LobbyManager lobbyScript;


    void Start()
    {
        //init playerId
        var lobbyObject = GameObject.Find(LOBBY_OBJECT_NAME);
        lobbyScript = lobbyObject.GetComponent<LobbyManager>();
        var LoginPlayer = lobbyScript.LoginPlayer;
        string playerNickName = LoginPlayer.playerNickName;
        string playerModel = "P" + LoginPlayer.playerModel;
        Destroy(lobbyObject.gameObject);

        //spawn player
        spawner.GetComponent<PlayerSpawner>().Spawn(playerNickName, playerModel);

        //other init
        WarpedPlayer = null;
    }

    public void Warp(GameObject player, string target)
    {
        DontDestroyOnLoad(this.gameObject);
        WarpedPlayer = player;
        PhotonNetwork.LoadLevel(target);
    }

    //public override void OnJoinRoomFailed(short returnCode, string message)
    //{
    //    PhotonNetwork.CreateRoom(ROOM_NAME);
    //}

    //public override void OnJoinedRoom()
    //{
    //    PhotonNetwork.LoadLevel(GAME_SENCE);
    //}
}
