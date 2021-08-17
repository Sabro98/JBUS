using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    string playerID;

    public void SetPlayerID(string id)
    {
        playerID = id;
    }

    public string GetPlayerID()
    {
        return playerID;
    }
}
