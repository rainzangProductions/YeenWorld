using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayer : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;
    float crouchSpeed;
    //public Transform orientation;
    public float groundDrag;
    public float turnSpeed = 10f;
    //public bool allowStrafe;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public float moonWalkerMultiplier;
    public bool hasMoonWalker = true;
    bool justLanded;

    [Header("Crouching")]
    bool isCrouching;
    public Transform bodyGFX;
    float originalHeight;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("SFX")]
    public AudioClip landingSound;
    public AudioClip lunarJumpSound;

    float horInput;
    float vertInput;

    Vector3 moveDirection;
    Rigidbody rb;
    CapsuleCollider cc;
    SoundMaster mixer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        rb.freezeRotation = true;
        readyToJump = true;
        crouchSpeed = moveSpeed / 2.5f;
        originalHeight = GetComponent<CapsuleCollider>().height;
        mixer = FindObjectOfType<SoundMaster>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (grounded && readyToJump && !justLanded) {
            mixer.PlaySFXAtPosition(landingSound, transform.position);
            justLanded = true;
        }
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        SpeedControl();
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        //Turn();
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        MovePlayerNoRotation();
        //Turn();
    }
   void Turn()
    {
        //turn more slowly when airborne
        if(!readyToJump || !grounded)
        {
            float yaw = 0.5f * turnSpeed * Input.GetAxis("Horizontal");
            transform.Rotate(0, yaw, 0);
        }else
        {
            float yaw = turnSpeed * Input.GetAxisRaw("Horizontal");
            transform.Rotate(0, yaw, 0);
        }
    }
    void MyInput() {
        horInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Jump") && readyToJump && grounded && !isCrouching) {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetButton("Moon Walker") && readyToJump && grounded && !isCrouching) {
            readyToJump = false;
            LunarJump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;
            Crouch();
        }
    }
    /*void MovePlayerRotation()
    {
        float speed = isCrouching ? crouchSpeed : moveSpeed;
        //float horizontalScale = turnSpeed/moveSpeed; // adjust strafing speed here

        if (!allowStrafe)
        {
            moveDirection = transform.forward * vertInput;
        }
        else
        {
            moveDirection = transform.forward * vertInput +
                            transform.right * horInput * horizontalScale;
        }
        moveDirection = transform.forward * vertInput;

        if (grounded)
            rb.AddForce(moveDirection * speed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection * speed * 10f * airMultiplier, ForceMode.Force);
    }*/

    void MovePlayerNoRotation() {
        float speed = isCrouching ? crouchSpeed : moveSpeed;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * vertInput + right * horInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);

        // rotate player to face movement direction
        if (moveDirection.magnitude > 0.1f) {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    void Jump() {
        //reset y velocity
        justLanded = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void LunarJump() {
        justLanded = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce * moonWalkerMultiplier, ForceMode.Impulse);
        mixer.PlaySFXAtPosition(lunarJumpSound, transform.position);
    }

    public void ResetJump()
    {
        readyToJump = true;
    }
    void Crouch()
    {
        if (isCrouching)
        {
            cc.height = originalHeight * 0.65f;
            bodyGFX.localScale = new Vector3(1, 0.65f, 1);
            //Debug.Log("crouched");
        }
        else
        {
            cc.height = originalHeight;
            bodyGFX.localScale = Vector3.one;
            //Debug.Log("uncrouched");
        }
    }
}