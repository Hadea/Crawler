using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySuicideBombAttack : MonoBehaviour, IEnemyAttackComponent
{
    public float triggerRadius = 3f;
    public int explosionDamage = 4;
    public float detonationRadius = 5f;
    public LayerMask raycastMask;
    public GameObject explosionEffectPrefab;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, detonationRadius);

        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, triggerRadius);
#endif
    }

    public void Attack(Ray ray, float distance)
    {
        if (distance < triggerRadius)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detonationRadius, raycastMask, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < colliders.Length; i++)
            {
                Health target = colliders[i].GetComponent<Health>();
                if (target != null)
                {
                    target.TakeDamage(explosionDamage);
                    if (explosionEffectPrefab != null)
                    {
                        GameObject newBullet = Instantiate(explosionEffectPrefab, transform.position, Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)));
                    }
                    GetComponent<Health>().Die();
                }
            }
        }
    }
}
