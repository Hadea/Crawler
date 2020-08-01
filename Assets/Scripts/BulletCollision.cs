using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BulletCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // check if something with health has been hit
        Health health = other.transform.GetComponent<Health>();
        if (health != null)
        {
            // deal damage to target
            health.TakeDamage();
        }

        // destroy bullet
        Destroy(gameObject);
    }
}
