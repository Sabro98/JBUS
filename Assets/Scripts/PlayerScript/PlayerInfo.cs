using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public JBUS_Player Player { get; set; }

    [SerializeField] TMP_Text playerIDText;

    const string SET_PLAYER_NAME = "SetPlayerName_RPC";

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        SetPlayerInfo(Player);
    }

    private void Update()
    {
        if (!PV.IsMine) return;
    }

    public void InitPlayerInfo(JBUS_Player player)
    {
        Player = player;
    }

    public bool IsMine()
    {
        return PV.IsMine;
    }

    //다른 세계에 존재하는 나의 이름도 바꿔주기 위함
    public void SetPlayerInfo(JBUS_Player player)
    {
        string nickname, model, id;
        int group;
        nickname = player.playerNickName;
        model = player.playerModel;
        id = player.playerID;
        group = player.playerGroup;

        PV.RPC(SET_PLAYER_NAME, RpcTarget.All, nickname, model, id, group);
    }

    [PunRPC]
    public void SetPlayerName_RPC(string nickname, string model, string id, int group)
    {
        Player = new JBUS_Player(nickname, model, id, group);
        playerIDText.text = Player.playerNickName;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PV.IsMine) return;
        base.OnPlayerEnteredRoom(newPlayer);
        PV.RPC(SET_PLAYER_NAME, RpcTarget.All, Player.playerNickName, Player.playerModel, Player.playerID, Player.playerGroup);
    }
}
