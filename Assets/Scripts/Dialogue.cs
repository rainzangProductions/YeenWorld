using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
//using UnityEngine.InputSystem.XR.Haptics;

//tutorial followed: https://www.youtube.com/watch?v=8oTYabhj248
//tutorial for reading from a file: https://youtu.be/LMw2M6lSkRw


public class Dialogue : MonoBehaviour
{
    public GameObject textPanel;
    public TextMeshProUGUI txtObj;
    string[] lines;
    float textSpeed = 0.05f;
    int index;
    TextAsset dialogueFile;
    string dialoguePath;

    public void StartDialogue()
    {
        //this selects the file and which language folder to get it from
        string gameLanguage = PlayerPrefs.GetString("languageCode");
        if (gameLanguage == null)
        {
            //default to English when interacting with NPCs for the first time
            gameLanguage = "EN";
            PlayerPrefs.SetString("languageCode", gameLanguage);
        }
        dialoguePath = $"NPCDialogue/{gameLanguage}/{gameObject.name}";
        dialogueFile = Resources.Load<TextAsset>(dialoguePath);

        //normal shtuffs
        lines = dialogueFile.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None); //this reads ahead every line in the file
        lines = System.Array.FindAll(lines, line => !string.IsNullOrWhiteSpace(line));
        index = 0;                                                                        //now we just start from the very beginning :0
        txtObj.text = string.Empty;                                                       //clears the textbox from the previous time we entered the interaction range, do NOT remove this
        textPanel.gameObject.SetActive(true);
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        //Debug.Log(dialoguePath);
        if(Input.GetButtonDown("Interact"))
        {
            if(txtObj.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                txtObj.text = lines[index];
            }
        }
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            txtObj.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            txtObj.text = string.Empty; //empty the text box, then start typing the next line
            StartCoroutine(TypeLine());
        }
        else
        {
            textPanel.gameObject.SetActive(false);
            gameObject.GetComponent<Dialogue>().enabled = false;
        }
    }
    public void ForceStop()
    {
        StopAllCoroutines(); 
        textPanel.gameObject.SetActive(false);
        gameObject.GetComponent<Dialogue>().enabled = false;
    }
}