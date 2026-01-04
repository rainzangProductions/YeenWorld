using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

    new public string name = "New Item";    // Name of the item
    public Sprite icon = null;              // Item icon
    public GameObject itemPrefab = null;
    public AudioClip useSound = null;       //Sound to play when you press on the item in your inven
    //public bool isDefaultItem = false;      // Is the item default wear?

    // Called when the item is pressed in the inventory
    public virtual void Use()
    {
        // Use the item
        // Something might happen

        Debug.Log("Using " + name);
    }

    public virtual void RemoveFromInventory()
    {
        Debug.Log(name + " is being thrown out !!");
        Inventory.instance.Remove(this);
    }

}