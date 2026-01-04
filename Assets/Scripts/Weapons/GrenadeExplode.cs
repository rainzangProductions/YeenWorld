using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplode : MonoBehaviour
{
    public int explosionForce = 10;
    public int explosionRange = 10;

    public int delay = 1;

    public GameObject explosionEffect;
    public AudioClip explosionSound;
    GameObject playerGrenadeLauncher;
    public EnemyAI enemyOwner;

    public enum ExplosionType { Grenade, Jelk }

    //Rigidbody rig;

    void Start()
    {
        //rig = GetComponent<Rigidbody>();
        playerGrenadeLauncher = GameObject.Find("GrenadeLauncherGun(Clone)");
        Invoke("DamageCalculation", delay);
    }

    //this happens anyways assuming the projectile will explode midair
    void DamageCalculation()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);

        foreach(Collider near in colliders)
        {
            //appplying physics to anything but other projectiles
            if (near.gameObject.tag != "Enemy Projectile" && near.gameObject.tag != "Player Projectile")
            {
                Rigidbody rb = near.GetComponent<Rigidbody>();
                if(rb != null) rb.AddExplosionForce(explosionForce, transform.position, explosionRange, 1f, ForceMode.Impulse);
            }
            //DEAL DAMAGE TO ENEMIES
            EnemyAI enemy = near.GetComponent<EnemyAI>();
            if (enemy != null && gameObject.tag == "Player Projectile")
            {
                int damageDealt = playerGrenadeLauncher.GetComponent<GrenadeLauncher>().thisWeapon.damage;
                enemy.TakeDamage(damageDealt);
            }
            //DEAL DAMAGE TO THE PLAYER
            PlayerHealth player = near.GetComponent<PlayerHealth>();
            if (player != null && gameObject.tag == "Enemy Projectile" && enemyOwner != null)
            {
                player.TakeDamage(enemyOwner.damage);
            }
        }
        Explode();
    }
    void Explode ()
    {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    //when the projectile hits literally anything
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            if (player != null && gameObject.tag == "Enemy Projectile" && enemyOwner != null)
            {
                player.TakeDamage(enemyOwner.damage);
                Explode();
            }
        }
        if(other.gameObject.GetComponent<Rigidbody>() != null) {
            other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange, 1f, ForceMode.Impulse);
            Explode();
        }
    }
}
    