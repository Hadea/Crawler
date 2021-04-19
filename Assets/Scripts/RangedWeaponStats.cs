using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRangedWeaponTemplate", menuName = "ScriptableObjects/RangedWeapon Template")]
public class RangedWeaponStats : Item
{
    [SerializeField]
    private GameObject projectilePrefab = null;

    [SerializeField]
    private int damage = 2;

    [SerializeField]
    private float projectileSpeed = 10;

    [SerializeField]
    private float shootingCooldown = 0.2f;
    public float cooldown { get { return shootingCooldown; } }

    [SerializeField]
    private float chargingTime = 1f;
    public float chargeTime { get { return chargingTime; } }

    [SerializeField]
    private int chargeDamageMultiplier = 2;

    [SerializeField]
    private float chargeSpeedMultiplier = 1.5f;

    public void Shoot(Vector3 position, Quaternion rotation, bool charged)
    {
        GameObject newBullet = Instantiate(projectilePrefab, position, rotation);
        Projectile bulletController = newBullet.GetComponent<Projectile>();
        bulletController.speed = projectileSpeed * (charged ? chargeSpeedMultiplier : 1f);
        bulletController.damage = damage * (charged ? chargeDamageMultiplier : 1);
    }
}
