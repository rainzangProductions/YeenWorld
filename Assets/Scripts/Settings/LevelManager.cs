using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    UnityEngine.SceneManagement.Scene scene;
    int sceneID;
    public Player playerScript;

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SetInt(string KeyName, int Value)
    {
        PlayerPrefs.SetInt(KeyName, Value);
    }

    public int GetInt(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName);
    }
    public void SaveGame()
    {
        scene = SceneManager.GetActiveScene();
        SetInt("currentLevel", scene.buildIndex);
        //playerScript.SavePlayer();
    }

    public void LoadGame()
    {
        sceneID = GetInt("currentLevel");
        SceneManager.LoadScene(sceneID, LoadSceneMode.Single);
        //playerScript.LoadPlayer();
        PlayerPrefs.SetString("justLoaded", "true");
    }
}