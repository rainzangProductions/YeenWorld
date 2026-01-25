using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//tutorial followed: https://youtu.be/mOqHVMS7-Nw
public class PlayerIdleFade : MonoBehaviour
{
    public float fadeSpeed, fadeAmount;
    float originalOpacity;
    Material[] Mats;
    public bool DoFade = false;
    public float idleTimer;
    public GameObject bodyGFX;
    public bool inWater;

    public Material opaqueRed;
    public Material fadeRed;
    Renderer rend;

    public ThirdPersonPlayer player;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        Mats = bodyGFX.GetComponent<Renderer>().materials;
        foreach (Material mat in Mats) {
            originalOpacity = mat.color.a;
        }
    }

    void Update()
    {
        //if(!Input.GetButton("Horizontal") || !Input.GetButton("Vertical"))
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            idleTimer = 0;
            DoFade = true;
        }
        else
        {
            idleTimer += Time.deltaTime;
        }
        if (idleTimer >= 3)
        {
            DoFade = false;
        }

        if (DoFade)
        {
            FadeNow();
            
        }
        else
        {
            ResetFade();
        }


        /*bool isMoving = player.moveDirection.magnitude > 0.1f;

        if (inWater && !isMoving) {
            // Fully solid in water when still
            rend.material = opaqueRed;
        } else if (isMoving) {
            // Fade while moving
            rend.material = fadeRed;
            //SetFadeAmount(0.4f); // example alpha
            Color currentColor = fadeRed.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            fadeRed.color = smoothColor;
        } else {
            rend.material = opaqueRed;
        }*/
    }
    void FadeNow()
    {
        foreach(Material mat in Mats)
        {
            Color currentColor = mat.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            mat.color = smoothColor;
            //mat.renderQueue = 2999;
        }
    }

    // Update is called once per frame
    void ResetFade()
    {
        foreach (Material mat in Mats)
        {
            Color currentColor = mat.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed * Time.deltaTime));
            mat.color = smoothColor;
            //mat.renderQueue = 3000;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) {
            inWater = true;
        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) {
            inWater = false;
        }
    }
}
