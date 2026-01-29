using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class LaseriteCannon : MonoBehaviour
{
    //Camera fpsCam;

    //layermask tutorial by: https://youtu.be/AECUU7BlRU4
    //int layerMask = 1 << 9;
    //layerMask =~ layerMask;
    //nametolayer layermask tutorial by: https://discussions.unity.com/t/how-to-ray-cast-through-objects/592754/3
    
    public GunItem thisWeapon;
    public GameObject bulletHole;
    public float bulletHoleDuration;

    InventoryUI inventory;
    SoundMaster mixer;

    void Start()
    {
        //fpsCam = Camera.main;
        inventory = FindObjectOfType<InventoryUI>();
        mixer = FindObjectOfType<SoundMaster>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !inventory.inventoryUI.activeSelf) Shoot();
    }

    void Shoot()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, thisWeapon.range);

        if (hits.Length == 0)
            return;

        // Sort hits by distance from camera
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        bool passedPlayer = false;

        foreach (RaycastHit hit in hits)
        {
            if (!passedPlayer)
            {
                if (hit.transform.CompareTag("Player"))
                {
                    passedPlayer = true;
                }
                continue;
            }

            // First thing AFTER the player
            Debug.Log("You shot at " + hit.transform.name);

            EnemyAI target = hit.transform.GetComponent<EnemyAI>();
            if (target != null)
                target.TakeDamage(thisWeapon.damage);
            //Instantiate(thisWeapon.impactParticle, hit.point, Quaternion.LookRotation(hit.normal));
            if (hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal * thisWeapon.impactForce);

            if (thisWeapon.useSound != null)
                mixer.PlaySFXAtPosition(thisWeapon.useSound, transform.position);

            BulletHoleImpact(hit);
            break;
        }
    }



    void BulletHoleImpact(RaycastHit hit) {
        Instantiate(thisWeapon.impactParticle, hit.point, Quaternion.LookRotation(hit.normal));
        GameObject impact = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
        impact.transform.SetParent(hit.transform);
        impact.transform.position += impact.transform.forward * 0.01f;
        float rot = Random.Range(0, 361);
        impact.transform.Rotate(0, 0, rot, Space.Self);
        Destroy(impact, bulletHoleDuration);
    }
}
