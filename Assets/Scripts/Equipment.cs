using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment
{
    [SerializeField]
    private Item[] currentEquipment;

    public int slotCount { get { return System.Enum.GetNames(typeof(Item.EquipmentTypes)).Length - 1; } }

    public delegate void OnEquipmentChanged(Item newItem, Item oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    public Equipment()
    {
        currentEquipment = new Item[slotCount];
    }

    public void Equip(Item newItem)
    {
        int slotIndex = (int)newItem.equipmentType - 1;

        if (slotIndex < 0)
        {
            return;
        }

        Item oldItem = currentEquipment[slotIndex];
        if (oldItem != null)
        {
            GameManager.instance.player1Stats.inventory.Add(oldItem);
        }

        currentEquipment[slotIndex] = newItem;

        onEquipmentChanged.Invoke(newItem, oldItem);
    }

    public void UnEquip(Item.EquipmentTypes equipmentType)
    {
        int slotIndex = (int)equipmentType - 1;
        if (slotIndex < 0)
        {
            return;
        }

        Item oldItem = currentEquipment[slotIndex];
        if (oldItem != null)
        {
            GameManager.instance.player1Stats.inventory.Add(oldItem);
            currentEquipment[slotIndex] = null;

            onEquipmentChanged(null, oldItem);
        }
    }

    public Item GetItem(int index)
    {
        return currentEquipment[index];
    }
}
