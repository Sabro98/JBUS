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
    [SerializeField] TMP_InputField IDInput;

    public string PlayerID { get; set; }
    public JBUS_Player LoginPlayer { get; set; }

    const string ROOM_NAME = "JBNU";
    const string GAME_SENCE = "Game";
    const string JOIN_SCENE = "Join";
    const string LOGIN_URL = "https://jbus.herokuapp.com/user/login";

    bool LoggedIn;

    private void Awake()
    {
        PlayerID = "";
        LoginPlayer = null;
        LoggedIn = false;
    }

    private void Start()
    {

    }

    public void join()
    {
        SceneManager.LoadScene(JOIN_SCENE);
    }

    public void Login()
    {
        PlayerID = IDInput.text;
        if (string.IsNullOrEmpty(PlayerID)) return;

        StartCoroutine(Login_REST());
    }

    IEnumerator Login_REST()
    {
        WWWForm form = new WWWForm();
        form.AddField("playerID", PlayerID);

        using (UnityWebRequest www = UnityWebRequest.Post(LOGIN_URL, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    string res = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    LoginPlayer = JBUS_Player.CreateFromJSON(res);
                    if(LoginPlayer.playerNickName != null) LoggedIn = true;

                    if (LoggedIn)
                    {
                        DontDestroyOnLoad(this.gameObject);
                        PhotonNetwork.JoinRoom(ROOM_NAME);
                    }
                }
            }
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(ROOM_NAME);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(GAME_SENCE);
    }
}
