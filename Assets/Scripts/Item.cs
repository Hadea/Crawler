using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : ScriptableObject
{
    public enum EquipmentTypes
    {
        None,
        MeeleWeapon,
        RangedWeapon,
        Armor,
    }

    public new string name;
    public EquipmentTypes equipmentType;

    public Sprite sprite;
    public GameObject prefab;

    public virtual void Use()
    {
        GameManager.instance.player1Stats.equipment.Equip(this);
        GameManager.instance.player1Stats.inventory.Remove(this);
    }
}
