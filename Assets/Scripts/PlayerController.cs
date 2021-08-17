using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] GameObject cameraHolder;

    const string walkAnim = "isWalk";
    const string jumpUpAnim = "isJumpUp";
    const string jumpingAnim = "isJumping";


    GameObject chatInputObject;
    TMP_InputField chatField;
    PlayerInfo playerInfo;

    float verticalLookRotation;

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

            //playerInfo 초기화
            playerInfo = GetComponent<PlayerInfo>();

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
                string msg = chatField.text;

                if(msg != "")
                {
                    playerInfo.Chat(msg);
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
