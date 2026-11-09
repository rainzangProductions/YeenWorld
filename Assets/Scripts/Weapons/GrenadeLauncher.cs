using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    Camera fpsCam;
    public GunItem thisWeapon;
    public int range;

    public Transform spawnPoint;
    public GameObject grenadePrefab;
    InventoryUI inventory;


    void Start()
    {
        fpsCam = Camera.main;
        //spawnPoint = GameObject.Find("SpawnPoint").transform;
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
        //Look in camera direction
        Transform TPSPlayer = GameObject.FindWithTag("Player").transform;
        Vector3 direction = Camera.main.transform.forward;
        direction.y = 0f;
        TPSPlayer.rotation = Quaternion.LookRotation(direction);
        TPSPlayer.GetComponent<ThirdPersonPlayer>().OnShoot();

        //GameObject grenadeInstance = Instantiate(grenadePrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject grenadeInstance = Instantiate(grenadePrefab, spawnPoint.position, spawnPoint.rotation);
        grenadeInstance.tag = "Player Projectile";
        grenadeInstance.transform.Rotate(90, 0, 0);
        grenadeInstance.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * range, ForceMode.Impulse);
        grenadeInstance.GetComponent<Rigidbody>().AddForce(spawnPoint.up * range/6, ForceMode.Impulse);
    }
}
