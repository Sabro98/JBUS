using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject textObject, GameMenuObj, chatInputObj, chatPanel;
    [SerializeField] GameObject ChangeChannelObj, BasicButtonsObj;
    [SerializeField] TMP_Dropdown ChannelDropDown;

    public static bool IsMenuActive { get; set; } //메뉴가 활성 상태인지

    const int CHAT_CAPACITY = 6;
    const string DISPLAY_CHAT = "DisplayChat_RPC";

    TMP_InputField chatField;
    List<Message> msgList;
    PhotonView PV;

    private void Awake()
    {
        msgList = new List<Message>();
        PV = GetComponent<PhotonView>();
        IsMenuActive = false;

        //커서 화면에 가두기
        Cursor.lockState = CursorLockMode.Locked;

        //채팅 박스 초기화
        chatField = chatInputObj.GetComponent<TMP_InputField>();
        chatInputObj.SetActive(false);

        //init dropdown
        initDropDown();
    }

    void initDropDown()
    {
        for(int i=0; i<ChannelDropDown.options.Count; i++)
        {
            var option = ChannelDropDown.options[i];

            if (option.text == PhotonNetwork.CurrentRoom.Name)
            {
                ChannelDropDown.value = i;
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerChatting.IsChatting) return;

            if (!GameMenuObj.activeSelf) ActiveMenu();
            else InActiveMenu();
            
        }
    }

    void ActiveMenu()
    {
        GameMenuObj.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        IsMenuActive = true;
    }

    void InActiveMenu()
    {
        ChangeChannelObj.SetActive(false);
        BasicButtonsObj.SetActive(true);
        GameMenuObj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        IsMenuActive = false;
    }

    public void OnQuitBtn()
    {
        GameManager.QuitGame();
    }

    public void OnChangeChannelBtn()
    {
        BasicButtonsObj.SetActive(false);
        ChangeChannelObj.SetActive(true);
    }

    public void OnChannelConfirmBtn()
    {
        var targetRoomName = ChannelDropDown.options[ChannelDropDown.value].text;
        InActiveMenu();

        gameObject.GetComponent<ChangeChannel>().Change(FindMyPlayer(), targetRoomName);
    }

    GameObject FindMyPlayer()
    {
        GameObject mine = null;
        foreach(var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!player.GetComponent<PlayerInfo>().IsMine()) continue;
            mine = player;
            break;
        }
        return mine;
    }

    public void OnChannelCancleBtn()
    {
        InActiveMenu();
    }

    //params -> (msg)
    public void DisplayChat(string msg)
    {
        PV.RPC(DISPLAY_CHAT, RpcTarget.All, msg);
    }

    public TMP_InputField GetChatField()
    {
        return chatField;
    }

    public GameObject GetChatInputObj()
    {
        return chatInputObj;
    }

    [PunRPC]
    void DisplayChat_RPC(string msg)
    {
        if (msgList.Count >= CHAT_CAPACITY)
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