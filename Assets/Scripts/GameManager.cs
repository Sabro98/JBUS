using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject spawner;

    const string PLAYER_OBJECT_NAME = "PlayerSpawnInfo";

    void Start()
    {
        //init playerId
        JBUS_Player CurrPlayer = InitPlayer();
        //spawn player
        SpawnPlayer(CurrPlayer);
    }

    JBUS_Player InitPlayer()
    {
        var lobbyObject = GameObject.FindWithTag(PLAYER_OBJECT_NAME);
        var lobbyScript = lobbyObject.GetComponent<ForPlayerSpawn>();
        var LoginPlayer = lobbyScript.SpawnPlayer;
        Destroy(lobbyObject.gameObject);

        if(LoginPlayer.playerModel[0] != 'P')
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
