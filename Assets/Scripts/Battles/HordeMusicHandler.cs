using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeMusicHandler : MonoBehaviour {
    public SoundMaster mixer;
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

    void Start() {
        pi = GetComponent<PlayerInteract>();
        mixer = FindObjectOfType<SoundMaster>();
    }
    public void StartMusic(int battleID) {
        if (battleID < 0 || battleID >= bossMusic.Length) {
            Debug.LogError("Invalid battleID for bossMusic array!");
            return;
        }

        //Debug.LogWarning("battleID " + battleID);
        mixer.StopMusic();
        mixer.PlayMusic(bossMusic[battleID-1]);

        if (customLoopStart)
            mixer.musicSource.time = loopStart;
        else
            mixer.musicSource.time = 0;
    }

    void Update() {
        //audioSource.clip
        if (mixer.musicSource.clip != null && mixer.musicSource.clip != mixer.bgMusic) {
            if (loopEnd > 0 && mixer.musicSource.time >= loopEnd) {
                mixer.musicSource.time = loopStart;
            }
        }
    }
}