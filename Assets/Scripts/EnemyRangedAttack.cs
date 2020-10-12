using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedAttack : MonoBehaviour, IEnemyAttackComponent
{
    public int projectileDamage = 1;
    public GameObject projectilePrefab;
    public Transform muzzlePosition;

    public float projectileSpeed = 30f;
    public float projectileCooldown = 1f;
    private float lastBulletFired;

    private NavMeshAgent agent;
    private EnemyControl control;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        control = GetComponent<EnemyControl>();
    }

    public void Attack(Ray ray, float distance)
    {
        Quaternion lookRotation = Quaternion.LookRotation(ray.GetPoint(distance) - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * control.rotationSpeed);

        if (Vector3.Distance(transform.position + transform.forward * distance, PlayerControl.player.transform.position) < 0.45f)
        {
            Debug.DrawLine(ray.origin, ray.GetPoint(distance), Color.green);
            agent.isStopped = true;
            if (Time.time > lastBulletFired + projectileCooldown)
            {
                Hit();
            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.GetPoint(distance), Color.blue);
        }
    }

    private void Hit()
    {
        lastBulletFired = Time.time;
        GameObject newBullet = Instantiate(projectilePrefab, muzzlePosition.position, muzzlePosition.rotation);
        BulletCollision bulletController = newBullet.GetComponent<BulletCollision>();
        bulletController.speed = projectileSpeed;
        bulletController.damage = projectileDamage;
    }
}
