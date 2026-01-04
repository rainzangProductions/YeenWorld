using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSettings : MonoBehaviour
{
    public string currentLanguage;
    public GameObject pauseMenu, optionsMenu;

    void Start()
    {
        ChangeLanguage(currentLanguage);
    }

    // Update is called once per frame
    void ChangeLanguage(string languageCode)
    {
        PlayerPrefs.SetString("languageCode", languageCode);
        Debug.LogError(PlayerPrefs.GetString("languageCode"));
    }
    void OpenSettings()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
}