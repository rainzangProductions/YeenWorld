using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public GameObject dialogueBox;
    public AudioClip pickupSound;
    public float audioVolume = 0.2f;

    int UIPosition;
    string[] lines;
    TextAsset File;
    string filePath;
    string gameLanguage;
    string translatedWord1;
    string translatedWord2;
    private Dictionary<string, string> firstHalf;
    private Dictionary<string, string> secondHalf;
    void Start()
    {
        gameLanguage = PlayerPrefs.GetString("languageCode");
        if (gameLanguage == null)
        {
            //default to English when interacting with NPCs for the first time
            gameLanguage = "EN";
            PlayerPrefs.SetString("languageCode", gameLanguage);
        }
        filePath = "KeyItems/" + gameObject.name;
        File = Resources.Load<TextAsset>(filePath);

        LoadLocalizationData();
        translatedWord1 = ParseText1();
        translatedWord2 = ParseText2();
    }
    void LoadLocalizationData()
    {
        firstHalf = new Dictionary<string, string>();
        secondHalf = new Dictionary<string, string>();

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

            if (parts.Length < 3)
            {
                Debug.LogWarning($"Skipping invalid line: {line}");
                continue;
            }

            string languageCode = parts[0].Trim();
            string textValue1 = parts[1].Trim();
            string textValue2 = parts[2].Trim();

            // Store in dictionary
            firstHalf[languageCode] = textValue1;
            secondHalf[languageCode] = textValue2;
        }
    }

    public string ParseText1()
    {
        if (firstHalf.TryGetValue(gameLanguage, out string value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning($"No translation found for language code: {gameLanguage}");
            return $"[{gameLanguage}]";
        }
    }
    public string ParseText2()
    {
        if (secondHalf.TryGetValue(gameLanguage, out string value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning($"No translation found for language code: {gameLanguage}");
            return $"[{gameLanguage}]";
        }
    }




    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="Player") StartCoroutine(KeyItemStuff());
    }

    // Update is called once per frame
    IEnumerator KeyItemStuff()
    {
        dialogueBox.gameObject.SetActive(true);
        dialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = translatedWord1;
        AudioSource.PlayClipAtPoint(pickupSound, transform.position, audioVolume);
        
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(4);

        dialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = translatedWord2;
        yield return new WaitForSecondsRealtime(5);

        dialogueBox.gameObject.SetActive(false);
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
