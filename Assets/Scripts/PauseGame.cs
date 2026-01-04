using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public bool paused;
    public GameObject pauseMenu;
    public AudioSource bgMusic;
    void Start()
    {
        paused = false;
    }
    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            //paused = !paused;
            if(paused) { Resume();}
            else {Pause();}
        }

        /*if(paused)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0;
        } else if(!paused)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {

        }*/
    }
    void Pause()
    {
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
        paused = true;
    }
    void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        paused = false;
    }
}