using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Building7Portal : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject InfoText, WarpManager;

    const string targetScene = "7th-floor-1";

    bool IsPlayerEnter;
    GameObject player;
  
    private void Awake()
    {
        IsPlayerEnter = false;
        InfoText.SetActive(IsPlayerEnter);
    }

    private void Update()
    {
        if (!IsPlayerEnter) return;
        if (Input.GetKey(KeyCode.E))
        {   
            var manager = WarpManager.GetComponent<WarpManager>();
            manager.Warp(player, targetScene);
            IsPlayerEnter = false;
            player.GetComponent<PlayerInfo>().DestroyPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerInfo>().IsMine()) return;
        
        IsPlayerEnter = true;
        player = other.gameObject;
        InfoText.SetActive(IsPlayerEnter);
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<PlayerInfo>().IsMine()) return;

        IsPlayerEnter = false;
        player = null;
        InfoText.SetActive(IsPlayerEnter);
    }
}
