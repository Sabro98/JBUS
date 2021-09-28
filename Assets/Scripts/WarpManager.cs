using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class WarpManager : MonoBehaviourPunCallbacks
{
    public JBUS_Player warpPlayer { get; set; }
    string to;
    bool IsWarp;

    private void Awake()
    {
        IsWarp = false;
    }

    public void Warp(GameObject player, string from, string to)
    {
        DontDestroyOnLoad(this.gameObject);
        IsWarp = true;
        var playerInfo = player.GetComponent<PlayerInfo>();
        warpPlayer = playerInfo.Player;
        this.to = to;
        SceneManager.LoadScene(JustLoadingScript.SceneName);
        string[] parms = new string[3] { playerInfo.Player.playerID, from, to };
        StartCoroutine(RestManager.WARP_FUNC, parms);
    }

    IEnumerator WarpPlayer_REST(string[] parms)
    {
        WWWForm form = new WWWForm();
        form.AddField("playerID", parms[0]);
        form.AddField("from", parms[1]);
        form.AddField("to", parms[2]);

        using (UnityWebRequest www = UnityWebRequest.Post(RestManager.WARP_URL, form))
        {
            yield return www.SendWebRequest();

            //로그인에 실패함
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("ERROR!!!!");
            }else if (www.isDone)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        if (!IsWarp) return;
        PhotonNetwork.JoinRoom(to);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (!IsWarp) return;
        PhotonNetwork.CreateRoom(to);
    }

    public override void OnJoinedRoom()
    {
        if (!IsWarp) return;
        PhotonNetwork.LoadLevel(to);
    }
}
