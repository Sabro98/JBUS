using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoginManager : MonoBehaviourPunCallbacks
{
    const string GAME_SCENE = "GAME";
    const string LOGIN_SCENE_NAME = "CreateAndJoinRooms";
    CreateAndJoinRooms loginScript;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        //로그인 처리
        GameObject loginScene = GameObject.Find(LOGIN_SCENE_NAME);
        loginScript = loginScene.GetComponent<CreateAndJoinRooms>();
        Destroy(loginScene);
        login();
    }

    void login()
    {
        string playerID = loginScript.playerID;

        //db에 접근 후 데이터 가져오는 작업 필요

        PhotonNetwork.LoadLevel(GAME_SCENE);
    }
    
}
