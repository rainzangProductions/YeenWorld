using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tutorial followed: https://youtu.be/mOqHVMS7-Nw
public class PlayerIdleFade : MonoBehaviour
{
    public float fadeSpeed, fadeAmount;
    float originalOpacity;
    Material[] Mats;
    public bool DoFade = false;
    public float idleTimer;
    public GameObject bodyGFX;
    
    void Start()
    {
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
    }
    void FadeNow()
    {
        foreach(Material mat in Mats)
        {
            Color currentColor = mat.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
            mat.color = smoothColor;
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
        }
    }
}
