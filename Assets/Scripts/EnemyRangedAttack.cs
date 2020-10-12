using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    public int projectileDamage = 1;
    public GameObject projectilePrefab;
    public Transform muzzlePosition;

    public float projectileSpeed = 30f;
    public float projectileCooldown = 1f;
    private float lastBulletFired;

    public void Attack()
    {
        if (Time.time > lastBulletFired + projectileCooldown)
        {
            lastBulletFired = Time.time;
            GameObject newBullet = Instantiate(projectilePrefab, muzzlePosition.position, muzzlePosition.rotation);
            BulletCollision bulletController = newBullet.GetComponent<BulletCollision>();
            bulletController.speed = projectileSpeed;
            bulletController.damage = projectileDamage;
        }
    }
}
