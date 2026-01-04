using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//Dave's Tutorial: https://youtu.be/UjkSFoLxesw
public class EnemyAI : MonoBehaviour
{
    [Header("Health")]
    public int health;
    public AudioClip[] deathSounds;

    public enum AttackType { Hitscan, Splash, Melee }
    public AttackType attackType;
    public int damage;

    [Header("Hitscan Attack Info")]
    public int scanDistance;
    [Header("Projectile Attack Info")]
    public GameObject projectile;
    public int projectileSpeed = 12;
    public bool useGravity = true;
    [Header("Melee Attack Info")]
    public int meleeRange;

    [Header("NavMesh")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Timing Info")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        /*if (doesFollowPlayer)
        {
            Vector3 targetPos = followTarget.transform.position;
            //Vector3 targetDir = this.transform.forward;
            transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, moveSpeed * Time.deltaTime);
            transform.LookAt(targetPos);
        }*/

        //Dave's tutorial
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) Chase();
        if (playerInSightRange && playerInAttackRange) Attack();
    }

    void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();
        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    void Chase()
    {
        agent.SetDestination(player.position);
    }
    void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if(!alreadyAttacked) { 
            if(attackType == AttackType.Hitscan)
            {

            }
            if (attackType == AttackType.Splash)
            {
                //grenades are the only projectiles I have rn lol
                //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.LookRotation(transform.up)).GetComponent<Rigidbody>();
                GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.LookRotation(transform.up));
                newProjectile.tag = "Enemy Projectile";
                if(newProjectile.GetComponent<GrenadeExplode>() != null)
                {
                    newProjectile.GetComponent<GrenadeExplode>().enemyOwner = this;
                }
                //rb.transform.Rotate(0, 0, -90);
                newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
                if (useGravity) newProjectile.GetComponent<Rigidbody>().AddForce(transform.up * (projectileSpeed / 4), ForceMode.Impulse);
            }
            if (attackType == AttackType.Melee)
            {

            }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0 )
        {
            int death = Random.Range(0, deathSounds.Length);
            AudioSource.PlayClipAtPoint(deathSounds[death], transform.position);
            //Invoke(nameof(Die), 0.25f);
            Die();
        }
    }

    void Die()
    {
        HordeBattle bb = GameObject.FindObjectOfType<HordeBattle>();
        //if(bb.inBattle) { bb.mobList.RemoveAt(0); }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}