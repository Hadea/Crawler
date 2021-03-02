using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeeleWeaponTemplate", menuName = "ScriptableObjects/MeeleWeapon Template")]
public class MeeleWeaponStats : Item
{
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float hittingCooldown = 0.1f;
    public float cooldown { get { return hittingCooldown; } }

    public int dmg { get { return damage; } }
}
