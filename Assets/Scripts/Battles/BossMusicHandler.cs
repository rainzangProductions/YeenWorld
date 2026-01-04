using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicHandler : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] bossMusic;
    //loop only accounts for 1 song rn, come back to this later
    public float loopStart;
    public float loopEnd;

    public bool customLoopStart;
    //public float volume;

    [Header("Boss Battle Info (DON'T TOUCH)")]
    public int activeMobSpawners;
    PlayerInteract pi;
    HordeBattle lastSpawnerInfo;
    //PI stores the ID of the last used mob spawner, whereas lastSpawnerInfo is any
    //new spawner the player walks into

    void Start()
    {
        pi = GetComponent<PlayerInteract>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mob Spawner")
        {
            lastSpawnerInfo = other.GetComponent<HordeBattle>();
            StartMusic(lastSpawnerInfo.battleID);

            /*lastSpawnerInfo = other.GetComponent<HordeBattle>();
            if (lastSpawnerInfo.battleID != pi.lastBattleID)
            {
                
                StartMusic(lastSpawnerInfo.battleID);
                pi.lastBattleID = lastSpawnerInfo.battleID;
                Debug.Log(pi.lastBattleID);
            }
            else
            {
                Debug.Log("you walked into a mob spawner of the same ID!");
            }*/
        }
    }
    void StartMusic(int battleID)
    {
        // Set the initial playback position to the loop start position
        audioSource.clip = bossMusic[lastSpawnerInfo.battleID - 1];
        if(customLoopStart) audioSource.time = loopStart;
        if (!customLoopStart)  audioSource.time = 0;
        //audioSource.volume = volume;
        audioSource.Play();
    }

    void Update()
    {
        //audioSource.clip
        if(audioSource.clip != null)
        {
            if (audioSource.time >= loopEnd || audioSource.time >= audioSource.clip.length)
            {
                // Set the time to the start of the loop section
                audioSource.time = loopStart;
            }
        }
    }
}