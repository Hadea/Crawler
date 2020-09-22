using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public RangedWeaponStats stats;
    public Transform muzzle;

    public void Shoot(bool charged)
    {
        stats.Shoot(muzzle.position, muzzle.rotation, charged);
    }
}
