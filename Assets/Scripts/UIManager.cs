using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    public int ChatCapacity = 6;

    [SerializeField] GameObject chatPanel, textObject;

    List<Message> msgList;
    PhotonView PV;

    const string DISPLAY_CHAT = "DisplayChat_RPC";

    private void Awake()
    {
        msgList = new List<Message>();
        PV = GetComponent<PhotonView>();
    }

    //params -> (msg)
    public void DisplayChat(string msg)
    {
        PV.RPC(DISPLAY_CHAT, RpcTarget.All, msg);
    }

    [PunRPC]
    void DisplayChat_RPC(string msg)
    {
        if (msgList.Count >= ChatCapacity)
        {
            Destroy(msgList[0].textObject.gameObject);
            msgList.Remove(msgList[0]);
        }

        Message newMessage = new Message();

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<TMP_Text>();
        newMessage.textObject.text = msg;

        msgList.Add(newMessage);

    }
}

[System.Serializable]
public class Message
{
    public TMP_Text textObject;
}