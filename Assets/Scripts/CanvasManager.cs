using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public GameObject chatPanel, chatInputObj, GameMenuObj;

    public int ChatCapacity = 6;
    public TMP_InputField chatField;

    const string DISPLAY_CHAT = "DisplayChat_RPC";
    const string CHAT_CANVAS = "Canvas";
    const string TEXT_OBJECT = "ChatText";

    GameObject textObject;
    List<Message> msgList;
    PhotonView PV;

    private void Awake()
    {
        //채팅 요소들 초기화
        initChat();

        msgList = new List<Message>();
        PV = GetComponent<PhotonView>();

        //커서 화면에 가두기
        Cursor.lockState = CursorLockMode.Locked;

        //채팅 박스 초기화
        chatField = chatInputObj.GetComponent<TMP_InputField>();
        chatInputObj.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerChatting.IsChatting) return;

            GameMenuObj.SetActive(!GameMenuObj.activeSelf);
            if (GameMenuObj.activeSelf) Cursor.lockState = CursorLockMode.Confined;
            else Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnQuitBtn()
    {
        GameManager.QuitGame();
    }

    void initChat()
    {
        textObject = Instantiate(Resources.Load(TEXT_OBJECT)) as GameObject;
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