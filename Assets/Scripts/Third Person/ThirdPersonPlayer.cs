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
    public bool allowStrafe;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public bool isCrouching;
    public Transform bodyGFX;
    float originalHeight;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    float horInput;
    float vertInput;

    Vector3 moveDirection;
    Rigidbody rb;
    CapsuleCollider cc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        rb.freezeRotation = true;
        readyToJump = true;
        crouchSpeed = moveSpeed / 2.5f;
        originalHeight = GetComponent<CapsuleCollider>().height;
    }

    void Update()
    {
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
        MovePlayer();
        Turn();
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
    void MyInput()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Jump") && readyToJump && grounded && !isCrouching)
        {
            readyToJump = false;
            Jump();
            //Debug.Log("jumped");
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;
            Crouch();
            //Debug.Log("crouch button pressed");
        }
    }

    /*void MovePlayer()
    {
        //put the ability to strafe or not AS A MAIN MENU OPTION
        if (!allowStrafe)
        {
            moveDirection = transform.forward * vertInput;
        }
        else
        {
            moveDirection = transform.forward * vertInput +
                            transform.right * horInput * 0.1f;
        }

        float speed = isCrouching ? crouchSpeed : moveSpeed;

        if (grounded)
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
    }*/
    void MovePlayer()
    {
        float speed = isCrouching ? crouchSpeed : moveSpeed;
        float horizontalScale = turnSpeed/moveSpeed; // adjust strafing speed here

        if (!allowStrafe)
        {
            moveDirection = transform.forward * vertInput;
        }
        else
        {
            moveDirection = transform.forward * vertInput +
                            transform.right * horInput * horizontalScale;
        }

        if (grounded)
            rb.AddForce(moveDirection * speed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection * speed * 10f * airMultiplier, ForceMode.Force);
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
    void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
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