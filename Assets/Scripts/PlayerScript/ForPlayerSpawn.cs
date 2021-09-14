using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForPlayerSpawn : MonoBehaviour
{
    public JBUS_Player SpawnPlayer { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
