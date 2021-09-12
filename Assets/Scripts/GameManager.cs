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

    void Start()
    {
        //init playerId
        JBUS_Player CurrPlayer = InitPlayer();
        //spawn player
        SpawnPlayer(CurrPlayer);
    }

    JBUS_Player InitPlayer()
    {
        var lobbyObject = GameObject.Find(LOBBY_OBJECT_NAME);
        LobbyManager lobbyScript = lobbyObject.GetComponent<LobbyManager>();
        var LoginPlayer = lobbyScript.LoginPlayer;
        Destroy(lobbyObject.gameObject);

        LoginPlayer.playerModel = "P" + LoginPlayer.playerModel;
        return LoginPlayer;
    }

    void SpawnPlayer(JBUS_Player currPlayer)
    {
        spawner.GetComponent<PlayerSpawner>().Spawn(currPlayer);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
