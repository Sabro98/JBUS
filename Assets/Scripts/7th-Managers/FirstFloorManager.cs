using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FirstFloorManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject spawner;

    private void Start()
    {
        spawner.GetComponent<PlayerSpawner>().Spawn();
    }
}
