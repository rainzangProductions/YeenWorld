using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ShipMovement : MonoBehaviour {

    [Header("Movement")]
    public float baseSpeed = 100;
	float Speed;
	public float turnSpeed = 60f;

    [Header("Booster")]
    public bool isBoosting;
	public bool canBoost;
	public float boostMultiplier = 2;
	float boostSpeed;

    [Header("Booster Stamina")]
    public float Stamina = 100;
	float maxStamina;
	public float staminaMultiplier = 0.15f;
	public Text staminaCounter;
	int staminaToDisplay;
	bool canRegen;

	[Header("Booster Cooldown")]
	float coolDown;
	public float coolDownTimer = 2f;

    [Header("Audio")]
    AudioSource audio;
	public AudioClip engineLoop;
	public AudioClip engineCooldown;
	public AudioClip wallHit;
	public float audioVolume;

    [Header("Wall Physics")]
    public Transform frontOfShipRaycast;
	bool atWall;
	public float wallCheckDistance = 5f;

	//private float yaw;
	//private float roll;
	//private float pitch;
	//private float accelerationAxis;
	//private float boostAxis;

    void Start() {
		audio = GetComponent<AudioSource>();
		maxStamina = Stamina;
		boostSpeed = baseSpeed * boostMultiplier;
		coolDown = coolDownTimer;
	}
    void Update () {
		WallCheck();
        ///doing the physics input here

        staminaToDisplay = (int)Stamina;
        staminaCounter.text = "Boost: " + staminaToDisplay.ToString();

        //cooldowntimer goes down DO NOT TOUCH THIS
        coolDownTimer -= Time.deltaTime;
		if (coolDownTimer >= 0.0f) canBoost = false;
		if (coolDownTimer <= 0.0f) canBoost = true;

		if (Stamina < maxStamina)
			//simple if statement allowing regen
			canRegen = true;
		
		if (Stamina <= 0) {
			//active cooldown by setting the timer to the base cooldown
			coolDownTimer = coolDown;
			audio.PlayOneShot(engineCooldown, audioVolume);

		} else if (Stamina >= maxStamina){
            //cap out stamina and disable regen
            //only checking when the stamina is above 100 bc
            //the regen must be disabled regardless
            canRegen = false;
            Stamina = maxStamina;
			
		}

		if (isBoosting) {
			//use boost. it has to be multiplied by 2 upon build bc the boost happens under fixedUpdate
			Stamina -= staminaMultiplier * 2;
		} else if (!isBoosting && Stamina < maxStamina && canRegen) {
			//recover boost. canBoost basically caps so you cant go over 100 when you're resting
			Stamina += staminaMultiplier;
		} else {
			
		} 
	}
    void FixedUpdate()
    {
        //replaced deltaTime with
        //fixedDeltaTime as well
        Thrust();
        Turn();
    }
    /*void GetInput()
    {
        yaw = turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        pitch = turnSpeed * Time.deltaTime * Input.GetAxis("Pitch");
        roll = turnSpeed * Time.deltaTime * Input.GetAxis("Roll");
        accelerationAxis = Input.GetAxis("Accelerate");
		//boostAxis = Input.GetAxis("Turbo");
    }*/
    void WallCheck()
    {
		RaycastHit hit;
		Vector3 forward = frontOfShipRaycast.TransformDirection(Vector3.forward);
        if (Physics.Raycast(frontOfShipRaycast.position, -forward, out hit, wallCheckDistance))
		{
			//this ensures that you only play the sound
			//whenever you run into a new wall
			if(!atWall) audio.PlayOneShot(wallHit, audioVolume*2.2f);
            atWall = true;
		}
		else
		{
			atWall = false;
		}
    }
	void Thrust() {
        if (Input.GetAxis("Accelerate") > 0 && !atWall)
        //if (accelerationAxis > 0 && !atWall)
        {
            transform.position += transform.forward * Speed * Time.deltaTime * Input.GetAxis("Accelerate");
            //transform.position += transform.forward * Speed * Time.deltaTime * accelerationAxis;
        }
        else
        {
            //UNABLE TO BOOST WHEN IDLING, future me keeps wondering how i made it work lol
            Speed = baseSpeed;
			canBoost = false;
            isBoosting = false;
        }

        if (Input.GetButton ("Turbo") && canBoost && !atWall) {
        //if (boostAxis > 0 && canBoost && !atWall) {
			Speed = boostSpeed;
			isBoosting = true;
			Debug.Log ("pressing space");
		} else {
			//turn off boost when not pressing the button
			Speed = baseSpeed;
			isBoosting = false;
		}
	}
	void Turn() {
        float yaw = turnSpeed * Time.deltaTime * Input.GetAxis ("Horizontal");
		float pitch = turnSpeed * Time.deltaTime * Input.GetAxis ("Pitch");
		float roll = turnSpeed * Time.deltaTime * Input.GetAxis ("Roll");
        transform.Rotate (-pitch, yaw, -roll);
	}
}