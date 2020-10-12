using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Image icon;
    private Button toggle;
    private Item item;
    private InventoryUI inventoryUI;

    void Awake()
    {
        toggle = GetComponent<Button>();
        icon = transform.GetChild(0).GetComponent<Image>();
        inventoryUI = GetComponentInParent<InventoryUI>();
        ClearSlot();
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.enabled = true;
        toggle.interactable = true;
        icon.sprite = item.sprite;
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        icon.enabled = false;
        toggle.interactable = false;
    }

    public void Select()
    {
        toggle.interactable = false;
        inventoryUI.SelectItem(item);
    }
}
