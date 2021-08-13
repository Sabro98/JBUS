using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] GameObject cameraHolder, chatBubble;

    const string walkAnim = "isWalk";
    const string jumpUpAnim = "isJumpUp";
    const string jumpingAnim = "isJumping";
    const string UPDATE_CHAT_BUBBLE = "UpdateChatBubble_RPC";

    const int damp = 5; // chat bubble's damp


    UIManager uiManager;

    GameObject chatInputObject;
    TMP_InputField chatField;

    float verticalLookRotation;
    float chatBubbleTime;

    bool grounded;
    bool jumpUp;
    bool jumping;
    bool isChatting;

    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;
    Animator animator;
    PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();


        //UIManager 초기화
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
        else
        {
            //커서 화면에 가두기
            Cursor.lockState = CursorLockMode.Locked;

            //채팅 박스 초기화
            chatInputObject = GameObject.Find("Canvas/ChatBubble");
            chatField = chatInputObject.GetComponent<TMP_InputField>();
            chatInputObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine) return;

        Chat();
        Look();
        Move();
        Jump();

        //rotate other player's chat bubbles look to current player
        //reference: https://answers.unity.com/questions/22130/how-do-i-make-an-object-always-face-the-player.html
        GameObject[] chatBubbles = GameObject.FindGameObjectsWithTag("chatBubble");
        Transform targetPosition = cameraHolder.transform;

        foreach (GameObject otherChatBubble in chatBubbles)
        {
            if (otherChatBubble == chatBubble) continue;
            var rotationAngle = Quaternion.LookRotation(( targetPosition.position - otherChatBubble.transform.position).normalized); // we get the angle has to be rotated
            Vector3 angle = rotationAngle.eulerAngles;
            angle.x = 0;
            angle.z = 0;
            rotationAngle = Quaternion.Euler(angle);
            rotationAngle *= Quaternion.Euler(0, 180, 0);
            otherChatBubble.transform.rotation = Quaternion.Slerp(otherChatBubble.transform.rotation, rotationAngle, Time.deltaTime * damp); // we rotate the rotationAngle 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!PV.IsMine) return;

        if (!grounded)
        {
            grounded = true;
            jumpUp = false;
            jumping = false;
            animator.SetBool(jumpUpAnim, false);
            animator.SetBool(jumpingAnim, false);
        }
    }

    void Chat()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isChatting)
            {
                string playerID = this.GetComponent<PlayerInfo>().GetPlayerID();
                string msg = chatField.text;

                if(msg != "")
                {
                    chatBubbleTime = 3f;
                    //다른 세계의 자신에게도 채팅을 띄우도록
                    PV.RPC(UPDATE_CHAT_BUBBLE, RpcTarget.All, msg);

                    uiManager.DisplayChat(playerID + " : " + msg);
                    chatField.text = "";
                }

                chatField.ActivateInputField();
                chatField.Select();
                chatInputObject.SetActive(false);

                isChatting = false;
            }
            else
            {
                isChatting = true;

                chatInputObject.SetActive(true);
                chatField.ActivateInputField();
                chatField.Select();
            }
        }

        //esc를 눌렀을 때 채팅 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isChatting)
            {
                chatField.text = "";
                chatField.ActivateInputField();
                chatField.Select();
                chatInputObject.SetActive(false);
                isChatting = false;
            }
        }

        UpdateChatBubbleTime();
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

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"),
            vertical = Input.GetAxis("Vertical");

        //채팅중에는 이동을 못하도록 -> 부드럽게 멈추도록 처리가 필요할듯
        if(isChatting)
        {
            horizontal = 0f;
            vertical = 0f;
        }

        Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

        if(horizontal == 0f && vertical == 0f) animator.SetBool(walkAnim, false);
        else animator.SetBool(walkAnim, true);
    }

    void Jump()
    {
        //채팅 중에는 점프를 못하도록
        if (isChatting) return;

        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
            grounded = false;
            jumpUp = true;
            animator.SetBool(jumpUpAnim, jumpUp);
        }

    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        if (jumpUp)
        {
            jumpUp = false;
            jumping = true;
            animator.SetBool(jumpingAnim, jumping);
        }
    }
}
