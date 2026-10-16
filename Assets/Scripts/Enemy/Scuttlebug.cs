using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

//Dave's Tutorial: https://youtu.be/UjkSFoLxesw
public class Scuttlebug : EnemyAI
{
    private string defaultColorClass = "red";
    public enum battleClass {Red, Blue};
    public battleClass currentBattleClass;
    Transform LOD0, LOD1, LOD2, LOD3; //SPECIFIC TO THE CURRENT MODEL OF THE SCUTTLEBUG
    void Start()
    {
        //is this switch necessary? The original idea was to change certain variables, like speed or attack,
        //therefore I really don't need this switch case or the ChangeColor() void in the distant future
        /*switch (currentBattleClass)
        {
            case battleClass.Red:
                break;
            case battleClass.Blue:
                ChangeColor();
                break;
        }*/
        ChangeColorAndStats();
    }
    //use this for more classes later on? but then we have to
    //configure where the material gameobject is for different enemies
    //should they also use battle classes/different tiers of difficulty
    void ChangeColorAndStats()
    {
        //the first half will be VISUAL ONLY
        string colorCode = currentBattleClass.ToString().ToLower();

        LOD0 = transform.Find("Mesh_LOD0");
        LOD1 = transform.Find("Mesh_LOD1");
        LOD2 = transform.Find("Mesh_LOD2");
        LOD3 = transform.Find("Mesh_LOD3");
        if(ColorUtility.TryParseHtmlString(colorCode, out Color color) && colorCode != defaultColorClass)
        {
            if(LOD0 != null)
        {
            LOD0.GetComponent<Renderer>().material.color = color;
        }
        if(LOD1 != null)
        {
            LOD1.GetComponent<Renderer>().material.color = color;
        }
        if(LOD2 != null)
        {
            LOD2.GetComponent<Renderer>().material.color = color;
        }
        if(LOD3 != null)
        {
            LOD3.GetComponent<Renderer>().material.color = color;
        }
        }
        base.timeBetweenAttacks *= .70f;
    }
    public override void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.LookRotation(transform.up));
            newProjectile.tag = "Enemy Projectile";
            if (newProjectile.GetComponent<GrenadeExplode>() != null) {
                newProjectile.GetComponent<GrenadeExplode>().enemyOwner = this;
            }
            //rb.transform.Rotate(0, 0, -90);
            newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            if (useGravity) newProjectile.GetComponent<Rigidbody>().AddForce(transform.up * (projectileSpeed / 4), ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    /*public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            mixer.PlaySFXAtPosition(deathSound, transform.position);
            //Invoke(nameof(Die), 0.25f);
            Die();
        }
    }*/

    public override void Patrol()
    {
        //Debug.Log("patrolling...");
        //base.Patrol();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}