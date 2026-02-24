using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tutorial: https://youtu.be/f473C43s8nE
public class ThirdPersonPlayer : MonoBehaviour {
    [Header("Movement")]
    public float moveSpeed;
    float airBlastSpeed;
    float crouchSpeed;
    //public Transform orientation;
    public float groundDrag = 5f;
    public float turnSpeed = 5f;
    //public bool allowStrafe;
    MovementState state;
    public bool hasMomentum;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    bool readyToJump;
    bool justLanded;

    //CHANGE hasMoonWalker to FALSE before the game releases!
    [Header("Moon Walker")]
    public float moonWalkerMultiplier;
    public bool hasMoonWalker = true;
    public float airMultiplier;
    public float airBlastForce;
    bool readyToAirBlast;
    bool isDashing;
    private bool airBlastedSinceLastLanding = false;

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
    public AudioClip airBlastSound;
    public AudioClip waterSplash;

    float horInput;
    float vertInput;

    bool inWater;

    public enum MovementState {
        moving,
        air
    }

    Vector3 moveDirection;
    Rigidbody rb;
    CapsuleCollider cc;
    SoundMaster mixer;

    void Start() {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        rb.freezeRotation = true;
        readyToJump = true;
        readyToAirBlast = true;
        crouchSpeed = moveSpeed / 2.5f;
        originalHeight = GetComponent<CapsuleCollider>().height;
        mixer = FindObjectOfType<SoundMaster>();
    }

    void Update() {
        if (Time.timeScale == 0f) return;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //if you land before the readytojump timer is reset
        //if (grounded && !readyToJump) ResetJump();

        if (grounded && readyToJump && !justLanded && !inWater) {
            mixer.PlaySFXAtPosition(landingSound, transform.position);
            justLanded = true;

            if (airBlastedSinceLastLanding) {
                hasMomentum = true;
                airBlastedSinceLastLanding = false; // reset after using
            }
        }
        
        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded) {
            rb.drag = isDashing ? 0f : groundDrag;
        } else {
            rb.drag = 0f;
        }
    }

    void FixedUpdate() {
        if (Time.timeScale == 0f) return;
        MovePlayer();
        //SpeedControl();
    }
   void Turn() {
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
        
        //JUMP
        if (Input.GetButton("Jump") && readyToJump && grounded && !isCrouching) {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        //DYNAMIC/CUT JUMP so you start falling earlier
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0) {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.65f, rb.velocity.z);
        }


        //Lunar Blast/Jump
        if (Input.GetButton("Moon Walker") && readyToJump && grounded && !isCrouching && !inWater) {
            readyToJump = false;
            LunarLaunch();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        //AIR BLAST
        if (Input.GetButton("Air Blast") && readyToAirBlast && !grounded && !isCrouching) {
            readyToAirBlast = false;
            AirBlast();
            Invoke(nameof(ResetAirBlast), jumpCooldown);
        }
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;
            Crouch();
        }
    }

    void MovePlayer() {
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
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,turnSpeed * Time.deltaTime);
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //Debug.Log(flatVel.magnitude);

        if (!grounded && hasMomentum)
            return;

        //BASICALLY JUST FOR THE AIRBLAST lmao. Enables a really shitty bhop
        if (flatVel.magnitude > moveSpeed && !grounded) {
            // smoothly lerp movementSpeed to desired value
            float time = 0f;
            float difference = Mathf.Abs(moveSpeed - flatVel.magnitude);
            float startValue = moveSpeed;
            //float startValue = flatVel.magnitude;

            while (time < difference) {
                moveSpeed = Mathf.Lerp(startValue, moveSpeed, time / difference);
                //time += Time.deltaTime * 50;
                time += Time.deltaTime;
            }
            //moveSpeed = flatVel.magnitude;
        }
        if (flatVel.magnitude > moveSpeed && !isDashing) {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            hasMomentum = false;
        }
    }

    void Jump() {
        //reset y velocity
        justLanded = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void LunarLaunch() {
        justLanded = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce * moonWalkerMultiplier, ForceMode.Impulse);
        mixer.PlaySFXAtPosition(lunarJumpSound, transform.position);
    }

    void AirBlast() {
        isDashing = true;
        mixer.PlaySFXAtPosition(airBlastSound, transform.position);

        Vector3 dash = transform.forward * airBlastForce;
        //airBlastDir = dash;

        rb.AddForce(dash, ForceMode.Impulse);
        hasMomentum = true;
        airBlastedSinceLastLanding = true;
    }

    public void ResetJump() {
        readyToJump = true;
    }
    public void ResetAirBlast() {
        isDashing = false;
        readyToAirBlast = true;
    }

    void StateHandler() {
        if(grounded) {
            state = MovementState.moving;
        } else {
            state = MovementState.air;
        }
    }

    void Crouch() {
        if (isCrouching)
        {
            cc.height = originalHeight * 0.65f;
            bodyGFX.localScale = new Vector3(1, 0.65f, 1);
        }
        else
        {
            cc.height = originalHeight;
            bodyGFX.localScale = Vector3.one;
        }
    }
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Water")) {
            inWater = true;
            mixer.PlaySFXAtPosition(waterSplash, transform.position);
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) {
            inWater = false;
        }
    }
}