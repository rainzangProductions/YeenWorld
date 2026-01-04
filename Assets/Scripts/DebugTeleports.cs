using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTeleports : MonoBehaviour
{
    public int targetAreaID;
    void LoadArea()
    {
        SceneManager.LoadScene(targetAreaID);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            LoadArea();
        }
    }
}