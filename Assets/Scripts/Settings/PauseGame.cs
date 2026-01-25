using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public bool paused;
    public bool inSettings;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public InventoryUI inventoryUI;
    public KeyItem keyItem;
    void Start()
    {
        paused = false;
        inventoryUI = FindObjectOfType<InventoryUI>();
        keyItem = FindObjectOfType<KeyItem>();
    }
    /*void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            //paused = !paused;
            if(paused) { Resume();}
            else {Pause();}
        }
    }*/
    void Update() {
        //cursorImg.transform.position = Input.mousePosition;

        if (Input.GetButtonDown("Pause")) {
            paused = !paused;
        }
        //you JUST pressed pause
        if (paused && !inSettings) {
            pauseMenu.SetActive(true);
            //cursorImg.SetActive(false);
            settingsMenu.SetActive(false);
            if(!keyItem.keyItemGet) Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //you are now in the SETTINGS
        } else if (paused && inSettings) {
            pauseMenu.SetActive(false);
            settingsMenu.SetActive(true);
            if (!keyItem.keyItemGet) Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            pauseMenu.SetActive(false);
            //cursorImg.SetActive(true);
            settingsMenu.SetActive(false);
            if (!keyItem.keyItemGet) Time.timeScale = 1;
            if (!inventoryUI.inventoryUI.activeSelf) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    public void SettingsButton() {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        inSettings = true;
    }
    public void SettingsBackButton() {
        pauseMenu.SetActive(true);
        //cursorImg.SetActive(false);
        settingsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inSettings = false;
    }
}