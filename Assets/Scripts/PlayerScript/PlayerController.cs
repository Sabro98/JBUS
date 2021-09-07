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

    float verticalLookRotation;

    bool grounded;
    bool jumpUp;
    bool jumping;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine) return;

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

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"),
            vertical = Input.GetAxis("Vertical");

        //채팅중에는 이동을 못하도록 -> 부드럽게 멈추도록 처리가 필요할듯
        if(PlayerChatting.IsChatting)
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
        if (PlayerChatting.IsChatting) return;

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
