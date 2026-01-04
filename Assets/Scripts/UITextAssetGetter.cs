using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class UITextAssetGetter : MonoBehaviour
{
    int UIPosition;
    string[] lines;
    TextAsset File;
    string filePath;
    string gameLanguage;
    private Dictionary<string, string> localizedText;
    void Start()
    {
        gameLanguage = PlayerPrefs.GetString("languageCode");
        if (gameLanguage == null)
        {
            //default to English when interacting with NPCs for the first time
            gameLanguage = "EN";
            PlayerPrefs.SetString("languageCode", gameLanguage);
        }
        filePath = "UI/" + gameObject.name;
        File = Resources.Load<TextAsset>(filePath);
        //Debug.Log(filePath);

        LoadLocalizationData();
        //Debug.Log($"Translation in {gameLanguage}: {translatedWord}");
        string translatedWord = ParseText();
        gameObject.GetComponent<TextMeshProUGUI>().text = translatedWord;
    }
    void LoadLocalizationData()
    {
        localizedText = new Dictionary<string, string>();

        if (File == null)
        {
            Debug.LogError("Localization file not assigned!");
            return;
        }

        string[] lines = File.text.Split('\n');

        foreach (string line in lines)
        {
            // Skip empty lines or comments
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            string[] parts = line.Split(',');

            if (parts.Length < 2)
            {
                Debug.LogWarning($"Skipping invalid line: {line}");
                continue;
            }

            string languageCode = parts[0].Trim();
            string textValue = parts[1].Trim();

            // Store in dictionary
            localizedText[languageCode] = textValue;
        }

        //Debug.Log($"Loaded {localizedText.Count} translations.");
    }

    public string ParseText()
    {
        if (localizedText.TryGetValue(gameLanguage, out string value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning($"No translation found for language code: {gameLanguage}");
            return $"[{gameLanguage}]";
        }
    }
}
