using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class WarpManager : MonoBehaviourPunCallbacks
{
    public JBUS_Player warpPlayer { get; set; }
    string m_to;
    string SceneName;
    bool IsWarp;

    private void Awake()
    {
        IsWarp = false;
    }

    public void Warp(GameObject player, string from, string to)
    {
        DontDestroyOnLoad(this.gameObject);
        IsWarp = true;
        //이동용 playerPrefab을 생성해서 해결할 필요가 있나 고려 -> 필요하다면 수정, 아니라면 유지 
        var playerInfo = player.GetComponent<PlayerInfo>();
        warpPlayer = playerInfo.Player;
        SceneManager.LoadScene(JustLoadingScript.SceneName);

        this.m_to = to + "--" + warpPlayer.playerGroup.ToString();
        this.SceneName = to;
        string[] parms = new string[3] { playerInfo.Player.playerID, from, m_to };
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
        PhotonNetwork.JoinRoom(m_to);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (!IsWarp) return;
        PhotonNetwork.CreateRoom(m_to);
    }

    public override void OnJoinedRoom()
    {
        if (!IsWarp) return;
        PhotonNetwork.LoadLevel(SceneName);
    }
}
