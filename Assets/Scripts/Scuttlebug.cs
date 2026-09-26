using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

//Dave's Tutorial: https://youtu.be/UjkSFoLxesw
public class Scuttlebug : EnemyAI
{

    public override void Attack()
    {
        if (!alreadyAttacked)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.LookRotation(transform.up));
            newProjectile.tag = "Enemy Projectile";
            if (newProjectile.GetComponent<GrenadeExplode>() != null)
            {
                newProjectile.GetComponent<GrenadeExplode>().enemyOwner = this;
            }
            //rb.transform.Rotate(0, 0, -90);
            newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            if (useGravity) newProjectile.GetComponent<Rigidbody>().AddForce(transform.up * (projectileSpeed / 4), ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            int death = Random.Range(0, deathSounds.Length);
            mixer.PlaySFXAtPosition(deathSounds[death], transform.position);
            //Invoke(nameof(Die), 0.25f);
            Die();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}