using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.Networking;

public class PlayerChatting : MonoBehaviour
{
    [SerializeField] GameObject chatBubble;

    const string UPDATE_CHAT_BUBBLE = "UpdateChatBubble_RPC";
    const int damp = 5; // chat bubble's damp

    public static bool IsChatting { get; set; }
    
    CanvasManager canvasManager;
    GameObject chatInputObject;
    TMP_InputField chatField;
    PhotonView PV;
    PlayerInfo playerInfo;

    float chatBubbleTime;

    private void Awake()
    {
        PV = gameObject.GetComponent<PhotonView>();
        playerInfo = gameObject.GetComponent<PlayerInfo>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!PV.IsMine) return;

        InitChatComponent();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine) return;
        Chat();

        //다른 사람의 ChatBubble을 회전시킴
        RotateOtherChatBubble();
    }

    void InitChatComponent()
    {
        //UIManager 초기화
        canvasManager = GameObject.FindWithTag("Canvas").GetComponent<CanvasManager>();
        chatInputObject = canvasManager.GetChatInputObj();
        chatField = canvasManager.GetChatField();
    }

    void Chat()
    {
        //플레이어가 생성한 ChatBubble의 유지시간 Update
        UpdateChatBubbleTime();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (IsChatting)
            {
                string msg = chatField.text;

                if (msg != "")
                {
                    chatBubbleTime = 3f;

                    string[] parms = new string[3] { playerInfo.Player.playerID, msg, PhotonNetwork.CurrentRoom.Name };
                    StartCoroutine(RestManager.CHAT_FUNC, parms);

                    //다른 세계의 자신에게도 채팅을 띄우도록
                    PV.RPC(UPDATE_CHAT_BUBBLE, RpcTarget.All, msg);

                    canvasManager.DisplayChat(playerInfo.Player.playerNickName + " : " + msg);
                    chatField.text = "";
                }

                chatField.ActivateInputField();
                chatField.Select();
                chatInputObject.SetActive(false);

                IsChatting = false;
            }
            else
            {
                IsChatting = true;

                chatInputObject.SetActive(true);
                chatField.ActivateInputField();
                chatField.Select();
            }
        }

        //esc를 눌렀을 때 채팅 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsChatting)
            {
                chatField.text = "";
                chatField.ActivateInputField();
                chatField.Select();
                chatInputObject.SetActive(false);
                IsChatting = false;
            }
        }
    }

    IEnumerator SaveChat_REST(string[] parms)
    {
        WWWForm form = new WWWForm();
        form.AddField("speaker", parms[0]);
        form.AddField("contents", parms[1]);
        form.AddField("location", parms[2]);

        using (UnityWebRequest www = UnityWebRequest.Post(RestManager.CHAT_URL, form))
        {
            yield return www.SendWebRequest();

            //로그인에 실패함
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("ERROR!!!!");
            }
        }
    }

    void RotateOtherChatBubble()
    {
        //rotate other player's chat bubbles look to current player
        //reference: https://answers.unity.com/questions/22130/how-do-i-make-an-object-always-face-the-player.html
        GameObject[] chatBubbles = GameObject.FindGameObjectsWithTag("chatBubble");
        Transform targetPosition = transform;

        foreach (GameObject otherChatBubble in chatBubbles)
        {
            if (otherChatBubble == chatBubble) continue;
            //otherChatBubble.transform.LookAt(targetPosition);
            var rotationAngle = Quaternion.LookRotation((targetPosition.position - otherChatBubble.transform.position).normalized); // we get the angle has to be rotated
            Vector3 angle = rotationAngle.eulerAngles;
            angle.x = 0;
            angle.z = 0;
            rotationAngle = Quaternion.Euler(angle);
            rotationAngle *= Quaternion.Euler(0, 180, 0);
            //otherChatBubble.transform.Rotate(Quaternion.Slerp(otherChatBubble.transform.rotation, rotationAngle, Time.deltaTime * damp).eulerAngles); // we rotate the rotationAngle )
            otherChatBubble.transform.rotation = Quaternion.Slerp(otherChatBubble.transform.rotation, rotationAngle, Time.deltaTime * damp); // we rotate the rotationAngle 
        }
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
}
