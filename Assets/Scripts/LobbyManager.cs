using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField IDInput;       //사용자 아이디 입력받는 input field
    [SerializeField] GameObject NormalComponent;   //평상시 보이는 요소들 (로그인 필드, 버튼 ..)
    [SerializeField] GameObject LoadingComponent;  //로딩중에 보여줄 글씨
    [SerializeField] TMP_Dropdown ChannelDropDown; //채널 선택 Component
    [SerializeField] GameObject ErrorText;         //에러 발생시 출력해줄 text 

    public string PlayerID { get; set; }

    const string GAME_SENCE = "Game";
    const string JOIN_SCENE = "Join";
    const string FAIL_TO_LOGIN_ERROR_MSG = "로그인에 실패하였습니다. 아이디를 다시 확인해 주세요.";
    const int ROOM_MAX_PLAYER = 16;
    const int GAME_DOSE_NOT_EXIT_ERROR_CODE = 32758;
    const int GAME_FULL_ERROR_CODE = 32765;

    string roomName;

    List<TMP_InputField> inputList;


    private void Awake()
    {
        PlayerID = "";
        roomName = "";
    }

    private void Start()
    {
        //input field list 초기화
        initInputList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnLogin();
        }
    }

    void initInputList()
    {
        inputList = new List<TMP_InputField>();
        inputList.Add(IDInput);

        //inputList[0].ActivateInputField();
        inputList[0].Select();
    }

    //Join 버튼을 누르면 해당 scene를 불러와줌
    public void OnJoin()
    {
        SceneManager.LoadScene(JOIN_SCENE);
    }

    //Login 버튼을 누르면 시도
    public void OnLogin()
    {
        PlayerID = IDInput.text;
        if (string.IsNullOrEmpty(PlayerID)) return; //유효성 검사
        
        roomName = ChannelDropDown.options[ChannelDropDown.value].text;

        DisplayLoading();
        StartCoroutine(RestManager.LOGIN_FUNC);
    }

    //로딩화면 출력
    void DisplayLoading()
    {
        NormalComponent.SetActive(false);
        LoadingComponent.SetActive(true);
    }

    //일반 화면을 출력
    void DisplayNormal()
    {
        NormalComponent.SetActive(true);
        LoadingComponent.SetActive(false);
    }

    //에러 메세지를 보여줌
    void DisplayErrorMsg(string msg)
    {
        ErrorText.SetActive(true);
        ErrorText.GetComponent<TMP_Text>().text = msg;
    }

    //REST를 보내 로그인 시도
    IEnumerator Login_REST()
    {
        WWWForm form = new WWWForm();
        form.AddField("playerID", PlayerID);

        using (UnityWebRequest www = UnityWebRequest.Post(RestManager.LOGIN_URL, form))
        {
            yield return www.SendWebRequest();

            //로그인에 실패함
            if (www.result != UnityWebRequest.Result.Success)
            {
                DisplayNormal();
                DisplayErrorMsg(FAIL_TO_LOGIN_ERROR_MSG);
            }
            else
            {
                if (www.isDone)
                {
                    string res = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    //플레이어 생성을 위한 prefab 만들어둠
                    var playerObj = Instantiate(Resources.Load("PlayerInfoObj")) as GameObject;
                    playerObj.GetComponent<ForPlayerSpawn>().SpawnPlayer = JBUS_Player.CreateFromJSON(res);
                    PhotonNetwork.JoinRoom(roomName);
                }
            }
        }
    }

    //Room join에 실패할 때 -> 스스로 방을 만들음
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if(returnCode == GAME_DOSE_NOT_EXIT_ERROR_CODE)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = ROOM_MAX_PLAYER;

            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

        //game full
        if (returnCode == GAME_FULL_ERROR_CODE)
        {
            DisplayNormal();
        }
    }

    //Room join에 성공하면 Game Scene을 불러옴
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(GAME_SENCE);
    }
}
