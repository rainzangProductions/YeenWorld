using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        //damage -= armor.GetValue();
        //damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + "damage.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.LogError(transform.name + " died.");
    }
}