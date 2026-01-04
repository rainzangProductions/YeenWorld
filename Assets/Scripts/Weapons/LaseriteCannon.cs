using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaseriteCannon : MonoBehaviour
{
    //Camera fpsCam;
    public GunItem thisWeapon;
    //int range;
    //int damage;
    InventoryUI inventory;

    void Start()
    {
        //fpsCam = Camera.main;
        inventory = FindObjectOfType<InventoryUI>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !inventory.inventoryUI.activeSelf)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //layermask tutorial by: https://youtu.be/AECUU7BlRU4
        //int layerMask = 1 << 9;
        //layerMask =~ layerMask;
        //nametolayer layermask tutorial by: https://discussions.unity.com/t/how-to-ray-cast-through-objects/592754/3
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, thisWeapon.range, ~(1 << LayerMask.NameToLayer("whatIsPlayer"))))
        {
            Debug.Log("You shot at " + hit.transform.name);

            EnemyAI target = hit.transform.GetComponent<EnemyAI>();
            if (target != null)
            {
                target.TakeDamage(thisWeapon.damage);
                Debug.Log(hit.transform.name + " took " + thisWeapon.damage.ToString() + " damage!");
            }

            Instantiate(thisWeapon.impactParticle, hit.point, Quaternion.LookRotation(hit.normal));

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * thisWeapon.impactForce);
            }
        }
    }
}
