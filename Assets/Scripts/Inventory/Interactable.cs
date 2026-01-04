using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // rework interacting with this tutorial in mind: https://youtu.be/gPPGnpV1Y1c?list=PLGUw8UNswJEOv8c5ZcoHarbON6mIEUFBC
    //also rework the radioInteract for the 3D overworld prompt messages
    public string promptMsg;

    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {
        //template function to be overrriden by subclasses
    }
}