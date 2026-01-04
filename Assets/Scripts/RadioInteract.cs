using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RadioInteract : MonoBehaviour
{
    public int songID = 0;
    //public int songVolume;
    bool radioOn = false;
    //AudioSource audioSource;
    public AudioClip[] radioSongs;
    public TextMeshPro frequency;
    
    public void Interact()
    {
        if(!radioOn) radioOn = true;
        if (radioOn)
        {
            if (songID == 0)
            {
                PlaySong();
                songID++;
            }
            else if (songID > 0 && songID <= radioSongs.Length - 1) {
                Destroy(GameObject.Find("One shot audio"));
                PlaySong();
                songID++;
            }

            // TURNING OFF THE RADIO
            else if (songID > radioSongs.Length - 1){
                Debug.Log("No more songs, turning off");
                Destroy(GameObject.Find("One shot audio"));
                radioOn = false;
                songID = 0;
                frequency.text = "";

            }
        }
    }

    // updating the text object to display the radio frequency
    void FixedUpdate()
    {
        if(radioOn)
        {
            switch (songID - 1)
            {
                case 0:
                    frequency.text = "Old Earth - 1080 kHz";
                    break;
                case 1:
                    frequency.text = "Old Earth - 88.7 MHz";
                    break;
                case 2:
                    //frequency.text = "O789MHz";
                    break;
                case 3:
                    break;
                default:
                    Debug.LogError("Radio couldn't play a song");
                    break;
            }
        }
    }
    void PlaySong()
    {
        AudioSource.PlayClipAtPoint(radioSongs[songID], new Vector3(4, -4.7f, -5.7f));
    }

}
