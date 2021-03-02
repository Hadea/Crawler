using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    public Item item { get; private set; }
    private Image icon;
    private Button toggle;
    private InventoryUI inventoryUI;
    public bool isEquipment;

    void Awake()
    {
        toggle = GetComponent<Button>();
        icon = transform.GetChild(0).GetComponent<Image>();
        inventoryUI = GetComponentInParent<InventoryUI>();
        Clear();
    }

    public void AddItem(Item newItem)
    {
        if (newItem == null)
        {
            throw new System.NullReferenceException();
        }
        item = newItem;
        icon.enabled = true;
        toggle.interactable = true;
        icon.sprite = item.sprite;
    }

    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        toggle.interactable = false;
    }

    public void Select()
    {
        toggle.interactable = false;
        inventoryUI.SelectItem(this, isEquipment);
    }

    public void Deselect()
    {
        if (item != null)
        {
            toggle.interactable = true;
        }
    }
}
