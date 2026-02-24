using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSettings : MonoBehaviour
{
    //public string currentLanguage;
    public GameObject pauseMenu, optionsMenu;
    int settingsLanguage;
    public GameObject dropDown;

    void Start()
    {
        string currentLanguage = PlayerPrefs.GetString("languageCode");
        ChangeLanguage(currentLanguage);
        
        if(currentLanguage == "EN") {
            dropDown.GetComponent<TMP_Dropdown>().value = 0;
        }
        if (currentLanguage == "ES") {
            dropDown.GetComponent<TMP_Dropdown>().value = 1;
        }
        if (currentLanguage == "FR") {
            dropDown.GetComponent<TMP_Dropdown>().value = 2;
        }
        if (currentLanguage == "DE") {
            dropDown.GetComponent<TMP_Dropdown>().value = 3;
        }
        if (currentLanguage == "FI") {
            dropDown.GetComponent<TMP_Dropdown>().value = 4;
        }
    }

    // Update is called once per frame
    public void ChangeLanguage(string languageCode)
    {
        //currentLanguage = languageCode;
        PlayerPrefs.SetString("languageCode", languageCode);
        Debug.LogError(PlayerPrefs.GetString("languageCode"));
    }
    void OpenSettings()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void SetSettingsLanguage() {
        settingsLanguage = dropDown.GetComponent<TMP_Dropdown>().value;
        switch (settingsLanguage) {
            case 0:
                ChangeLanguage("EN");
                break;
            case 1:
                ChangeLanguage("ES");
                break;
            case 2:
                ChangeLanguage("FR");
                break;
            case 3:
                ChangeLanguage("DE");
                break;
            case 4:
                ChangeLanguage("FI");
                break;
        }
        //Debug.LogWarning(dropDown.GetComponent<TMP_Dropdown>().value);
    }
}