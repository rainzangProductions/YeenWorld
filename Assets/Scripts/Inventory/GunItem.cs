using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Inventory/Gun")]
public class GunItem : Item
{
    public int damage;
    public int range;

    public GameObject impactParticle;
    public int impactForce;
    public GameObject playerPrefab;

    GameObject lastInstantiatedWeapon;

    public override void Use ()
    {
        Debug.Log(this.name + " was clicked on!");
        if (lastInstantiatedWeapon)
        {
            Destroy(lastInstantiatedWeapon);
            Debug.Log("item put away");
        }
        if (!lastInstantiatedWeapon)
        {
            lastInstantiatedWeapon = Instantiate(itemPrefab, GameObject.Find("WeaponHolster").transform.position, GameObject.Find("WeaponHolster").transform.rotation, GameObject.Find("WeaponHolster").transform);
            //Instantiate(itemPrefab, GameObject.Find("WeaponHolster").transform.position, GameObject.Find("WeaponHolster").transform.rotation, GameObject.Find("WeaponHolster").transform);
            Debug.Log("item created");
        }

    }
    public override void RemoveFromInventory()
    {
        if (GameObject.FindGameObjectWithTag("Weapon")) Debug.Log("weapon obj exists, deleting it now");
        Destroy(GameObject.FindGameObjectWithTag("Weapon"));
        Destroy(lastInstantiatedWeapon);
        Debug.Log(name + " is being thrown out !!");
        Inventory.instance.Remove(this);
    }
}
