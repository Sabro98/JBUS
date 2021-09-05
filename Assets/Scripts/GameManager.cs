using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject spawner;

    const string LOBBY_OBJECT_NAME = "LobbyManager";
    const string UI_MANAGER = "UIManager";

    LobbyManager lobbyScript;

    private void Awake()
    {

    }

    void Start()
    {
        //init playerId
        var lobbyObject = GameObject.Find(LOBBY_OBJECT_NAME);
        lobbyScript = lobbyObject.GetComponent<LobbyManager>();
        var LoginPlayer = lobbyScript.LoginPlayer;
        string playerID = LoginPlayer.playerID;
        string playerNickName = LoginPlayer.playerNickName;
        string playerModel = "P" + LoginPlayer.playerModel;
        Destroy(lobbyObject.gameObject);

        //spawn player
        spawner.GetComponent<PlayerSpawner>().Spawn(playerNickName, playerModel, playerID);
    }
}
