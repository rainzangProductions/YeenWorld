using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int level;
    public int health = 40;
    public float[] position;

    void Awake()
    {
        if (PlayerPrefs.GetString("justLoaded") == "true")
        {
            Debug.Log("JUST LOADED");
            LoadPlayer();
            PlayerPrefs.SetString("justLoaded", "false");
        }
        else
        {
            PlayerPrefs.SetString("justLoaded", "false");
        }
    }

    
    //any setting you want saved in the game? stats or object positions?
    //you need to reference everything here
    public void SavePlayer()
    {
        position[0] = gameObject.transform.position.x;
        position[1] = gameObject.transform.position.y;
        position[2] = gameObject.transform.position.z;
        PlayerPrefs.SetFloat("X", position[0]);
        PlayerPrefs.SetFloat("Y", position[1]);
        PlayerPrefs.SetFloat("Z", position[2]);
    }

    public void LoadPlayer()
    {
        //PlayerSettings settings = SaveSystem.LoadPlayer();

        //SceneManager.LoadScene(settings.level);
        //health = settings.health;

        //Vector3 position;
        //position.x = settings.position[0];
        // position.y = settings.position[1];
        // position.z = settings.position[2];
        //transform.position = position;
        transform.position = new Vector3(PlayerPrefs.GetFloat("X"), PlayerPrefs.GetFloat("Y"), PlayerPrefs.GetFloat("Z"));

    }
}