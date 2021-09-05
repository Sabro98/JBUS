using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstFloorManager : MonoBehaviourPunCallbacks
{
    [SerializeField] PlayerSpawner spawner;

    const string PREV_PORTAL_NAME = "WarpManager";
    const string UI_MANAGER = "UIManager";

    WarpManager prevManager;

    private void Awake()
    {

    }

    private void Start()
    {
        var prevManagerObj = GameObject.Find(PREV_PORTAL_NAME);
        prevManager = prevManagerObj.GetComponent<WarpManager>();
        var warpPlayer = prevManager.warpPlayer.Clone() as JBUS_Player;
        var currPlayerName = warpPlayer.playerNickName;
        var currPlayerModel = warpPlayer.playerModel;
        var currPlayerID = warpPlayer.playerID;

        spawner.Spawn(currPlayerName, currPlayerModel, currPlayerID);
        Destroy(prevManagerObj.gameObject);
    }
}
