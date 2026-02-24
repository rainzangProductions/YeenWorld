using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HordeBattle : MonoBehaviour
{
    [Header("Variables")]
    public bool inBattle;
    public int battleID;
    public HordeMusicHandler hmh;

    [Header("Mob Info")]
    public int mobsToSpawn;
    List<GameObject> mobList = new List<GameObject>();

    [Header("Prefab Info")]
    public GameObject spawnPointParentObj;
    List<Transform> spawnPoints = new List<Transform>();
    public GameObject[] mobPrefabs;

    void Start()
    {
        foreach(Transform child in spawnPointParentObj.transform)
        {
            spawnPoints.Add(child);
        }
    }

    /*void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            collider.GetComponent<PlayerInteract>().battleID = battleID;
        }
    }*/
    public void StartBattle()
    {
        if(!inBattle)
            //bmh.GetComponent<AudioSource>().mute = false;
        {
            Debug.Log("Type " + battleID + "Spawner Activated");
            for (int i = 0; i < mobsToSpawn; i++)
            {
                int nextSpawn = Random.Range(0, spawnPoints.Count);
                int nextMob = Random.Range(0, mobPrefabs.Length);
                GameObject newMob = Instantiate(mobPrefabs[nextMob], spawnPoints[nextSpawn].transform.position, Quaternion.identity);
                mobList.Add(newMob);
                //mobCount++;
            }

            hmh.StartMusic(battleID);
            inBattle = true;
            hmh.activeMobSpawners++;
            Debug.LogWarning(hmh.activeMobSpawners + " active mob spawners");
        }
    }

    void Update()
    {
        if (mobList.Count <= 0 && inBattle)
        {
            Debug.Log("BATTLE COMPLETED");
            
            hmh.activeMobSpawners--;
            Debug.LogWarning(hmh.activeMobSpawners + " active mob spawners");

            if (hmh.activeMobSpawners <= 0) {
                hmh.mixer.musicSource.Stop();
                hmh.mixer.musicSource.clip = hmh.mixer.bgMusic;
                hmh.mixer.musicSource.time = 0f;
                hmh.mixer.musicSource.Play();
            }

            inBattle = false;
        }
        //when a mob dies, it destroys its gameobject. therefore, I can't run any code after it dies
        //so the following code will be running in Update() to see if the mob list has any entries
        //of GOs that are now null. this only counts mobs spawned by its respective spawner
        for (int i = mobList.Count - 1; i >= 0; i--)
        {
            if (mobList[i] == null)
            {
                mobList.RemoveAt(i);
            }
        }
    }
}