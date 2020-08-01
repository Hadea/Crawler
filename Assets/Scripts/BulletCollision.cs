using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BulletCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject hitEffect = null;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if something with health has been hit
        Health health = other.transform.GetComponent<Health>();
        if (health != null)
        {
            // deal damage to target
            health.TakeDamage();
        }
        // spawn hit effect on current position
        float offset = rigid.velocity.magnitude;
        GameObject.Instantiate(hitEffect, transform.position - transform.forward * offset * 0.04f, transform.rotation * Quaternion.Euler(Vector3.up * 180f));

        // destroy bullet
        Destroy(gameObject);
    }
}
