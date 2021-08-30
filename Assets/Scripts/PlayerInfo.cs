using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject chatBubble;
    [SerializeField] TMP_Text playerIDText;

    const string UPDATE_CHAT_BUBBLE = "UpdateChatBubble_RPC";
    const string UPDATE_Player_ID = "UpdatePlayerID_RPC";
    const int damp = 5; // chat bubble's damp

    UIManager uiManager;
    PhotonView PV;

    float chatBubbleTime;

    public string PlayerID { get; set; }

    private void Awake()
    {
        //UIManager 초기화
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        PV = GetComponent<PhotonView>();

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!PV.IsMine) return;

        UpdateChatBubbleTime();

        //rotate other player's chat bubbles look to current player
        //reference: https://answers.unity.com/questions/22130/how-do-i-make-an-object-always-face-the-player.html
        GameObject[] chatBubbles = GameObject.FindGameObjectsWithTag("chatBubble");
        Transform targetPosition = transform;

        foreach (GameObject otherChatBubble in chatBubbles)
        {
            if (otherChatBubble == chatBubble) continue;
            otherChatBubble.transform.LookAt(targetPosition);
            //var rotationAngle = Quaternion.LookRotation((targetPosition.position - otherChatBubble.transform.position).normalized); // we get the angle has to be rotated
            //Vector3 angle = rotationAngle.eulerAngles;
            //angle.x = 0;
            //angle.z = 0;
            //rotationAngle = Quaternion.Euler(angle);
            //rotationAngle *= Quaternion.Euler(0, 180, 0);
            //otherChatBubble.transform.Rotate(Quaternion.Slerp(otherChatBubble.transform.rotation, rotationAngle, Time.deltaTime * damp).eulerAngles); // we rotate the rotationAngle )
            //otherChatBubble.transform.rotation = Quaternion.Slerp(otherChatBubble.transform.rotation, rotationAngle, Time.deltaTime * damp); // we rotate the rotationAngle 
        }
    }

    [PunRPC]
    public void UpdatePlayerID_RPC(string id)
    {
        PlayerID = id;
        playerIDText.text = PlayerID;
    }

    public void SetPlayerID(string id)
    {
        PV.RPC(UPDATE_Player_ID, RpcTarget.All, id);
    }

    public void Chat(string msg)
    {
        chatBubbleTime = 3f;

        //다른 세계의 자신에게도 채팅을 띄우도록
        PV.RPC(UPDATE_CHAT_BUBBLE, RpcTarget.All, msg);

        uiManager.DisplayChat(PlayerID + " : " + msg);
    }

    void UpdateChatBubbleTime()
    {
        if (chatBubbleTime <= 0f) return;

        chatBubbleTime -= Time.deltaTime;
        if (chatBubbleTime <= 0f)
            PV.RPC(UPDATE_CHAT_BUBBLE, RpcTarget.All, "");
    }

    [PunRPC]
    void UpdateChatBubble_RPC(string msg)
    {
        chatBubble.GetComponentInChildren<TMP_Text>().text = msg;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PV.IsMine) return;
        base.OnPlayerEnteredRoom(newPlayer);
        PV.RPC(UPDATE_Player_ID, RpcTarget.All, PlayerID);
    }
}
