using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class PlayerInteract : MonoBehaviour {

    //READING LINES FROM THE FILE
    public GameObject E2InteractPanel;
    public TextMeshProUGUI E2InteractTxt;
    string[] lines;
    TextAsset File;
    string filePath;
    //chatgpt
    string gameLanguage;
    private Dictionary<string, string> localizedText;

    NPCLookat npcl;
    Interactable npci;
    Dialogue dialogue;
    ItemPickup item;
    RadioInteract radioi;
    bool onlyLookAtPlayer = false;
    bool inNPCRange;

    public int lastBattleID;

    void Start()
    {
        //this selects the file and which language folder to get it from
        gameLanguage = PlayerPrefs.GetString("languageCode");
        if (gameLanguage == null)
        {
            //default to English when interacting with NPCs for the first time
            gameLanguage = "EN";
            PlayerPrefs.SetString("languageCode", gameLanguage);
        }
        filePath = "UI/" + "interactpopup";
        File = Resources.Load<TextAsset>(filePath);
        //chatgpt
        LoadLocalizationData();
        string translatedWord = GetText();
        //Debug.Log($"Translation in {gameLanguage}: {translatedWord}");
        E2InteractTxt.text = translatedWord;

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

        Debug.Log($"Loaded {localizedText.Count} translations.");
    }

    public string GetText()
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
    void Update()
    {
        if (inNPCRange && !onlyLookAtPlayer)
        {
            E2InteractPanel.SetActive(true);
        }
        else {
            E2InteractPanel.SetActive(false);
        }
        if(dialogue != null)
        {
            if (dialogue.enabled) E2InteractPanel.SetActive(false);
        }

        //INTERACTING WITH NPCS
        //stuff to happen without pressing any buttons
        if (inNPCRange)
        {
            if (!npcl.isInanimate)
            {
                npcl.LookAt(this.transform);
            }
            else
            {
                Debug.Log("no npcl found");
            }
        }

        
        
        //now by pressing buttons
        if (Input.GetButtonDown("Interact"))
        {
            if (inNPCRange)
            {
                E2InteractPanel.SetActive(false);
            }
            if (dialogue != null)
            {
                if (!dialogue.enabled)
                {
                    dialogue.enabled = true;
                    dialogue.StartDialogue();
                }
            }
            if(radioi != null) radioi.Interact();
        }
        if (item != null)
        {
            Debug.Log(item.name);
            //SetFocus(interactable);
            Debug.Log("iNTERACTABLE");
            item.Interact();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            Debug.Log("Interactable object triggered");
            inNPCRange = true;
        }
        item = other.GetComponent<ItemPickup>();
        npcl = other.GetComponent<NPCLookat>();
        if(npcl != null)
        {
            onlyLookAtPlayer = npcl.OnlyLookAtPlayer;
        }
        dialogue = other.GetComponent<Dialogue>();

        radioi = other.GetComponent<RadioInteract>();

        //BOSS BATTLES
        //BOSS BATTLES
        if (other.tag == "Mob Spawner")
        {
            if(!other.GetComponent<HordeBattle>().inBattle)
            {
                lastBattleID = other.GetComponent<HordeBattle>().battleID;
                //other.GetComponent<HordeBattle>().battleID = battleID;
                other.GetComponent<HordeBattle>().StartBattle();
                Debug.LogAssertion("Battle Started!");
            }
        }
        Debug.Log("entering battle zone: " + lastBattleID);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            Debug.Log("No longer in range of an interactable object");
            inNPCRange = false;
        }
        if (other.GetComponent<NPCLookat>() != null)
        {
            npcl = null;
        }
        if (other.GetComponent<Dialogue>() != null)
        {
            dialogue.ForceStop();
            dialogue = null;
        }
        if (other.GetComponent<ItemPickup>() != null)
        {
            item = null;
        }
        if (other.GetComponent<RadioInteract>() != null)
        {
            radioi = null;
        }
    }
}