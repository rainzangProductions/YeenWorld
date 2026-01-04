using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaUnlocks : MonoBehaviour
{
    #region

    // biome and then followed by subterrains
    // ocean biome
    //crashland area will have a few landmarks, most notably the crashland region

    #endregion

 //IN THIS ORDER SPECIFICALLY DOWN BELOW
    //public int areaID;
    public string Name;
    public string Description;
    public bool isUnlocked;

    public List<AreaUnlocks> Areas;
    void Awake()
    {
        //Areas.AddRange("Crashlanding", "Area where the main character crash landed", false);
    }
}