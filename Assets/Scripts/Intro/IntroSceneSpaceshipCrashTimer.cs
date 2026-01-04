using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class IntroSceneSpaceshipCrashTimer : MonoBehaviour
{
    public Text crashLandingTimer;
    public float Timer = 20f;
    public bool fadeStarted = false;
    public GameObject Panel2Fade;
    public float timeBetweenFades = 0.08f;
    public bool audioPlayed = false;
    public AudioClip throttle;
    public AudioClip countdown;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Timer -= Time.deltaTime;
        crashLandingTimer.text = "FUEL LOW !! CRASH LANDING \nIN : " + (int)Timer + " SECONDS";
        if(Timer <= 0 && !fadeStarted)
        {
            fadeStarted = !fadeStarted;
            StartCoroutine(FadeOut());
        }
        if(Timer <= 6 && !audioPlayed)
        {
            audioPlayed = !audioPlayed;
            audioSource.PlayOneShot(countdown, 0.2F);
        }
    }

    IEnumerator FadeOut()
    {
        audioSource.PlayOneShot(throttle, 0.9F);

        //fifteen intervals of 17
        Image image = Panel2Fade.GetComponent<Image>();
        image.color = new Color32(27, 15, 29, 17);

        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 34);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 51);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 68);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 85);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 102);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 119);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 136);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 153);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 170);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 187);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 204);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 221);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 238);
        yield return new WaitForSecondsRealtime(timeBetweenFades);
        image.color = new Color32(27, 15, 29, 255);
        yield return new WaitForSecondsRealtime(timeBetweenFades);

        SceneManager.LoadScene(2);
        yield return null;
    }
}
