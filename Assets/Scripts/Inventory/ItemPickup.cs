using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ItemPickup : MonoBehaviour {

    public Item item;   // Item to put in the inventory on pickup
    public AudioClip pickupSound;
    public float audioVolume = 1f;

    // When the player interacts with the item
    public void Interact()
    {
        PickUp();   // Pick it up!
    }

    // Pick up the item
    void PickUp()
    {
        Debug.Log("Picking up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);    // Add to inventory

        // If successfully picked up
        if (wasPickedUp)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, audioVolume);
            Destroy(gameObject);    // Destroy item from scene
        }
    }
    
}