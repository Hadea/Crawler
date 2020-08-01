using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 1;

    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage()
    {
        // reduce health by one life
        currentHealth--;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // destroy object
        currentHealth = 0;
        Destroy(gameObject);
    }
}
