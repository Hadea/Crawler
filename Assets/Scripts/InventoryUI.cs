using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Coroutine coroutine;

    public Transform inventorySlotsParent;
    private InventoryUISlot[] inventorySlots;

    public InventoryUISlot[] equipmentSlots;

    public InventoryUISlot selected { get; private set; }

    public Button removeButton;
    public Button equipButton;

    void Awake()
    {
        selected = null;

        GameManager.instance.player1Stats.inventory.onItemChanged += UpdateUI;
        GameManager.instance.player1Stats.equipment.onEquipmentChanged += UpdateUI;

        inventorySlots = inventorySlotsParent.GetComponentsInChildren<InventoryUISlot>(true);

        removeButton.interactable = false;

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Time.timeScale = 0f;// float.Epsilon;

        coroutine = StartCoroutine(Coroutine());
    }

    void OnDisable()
    {
        removeButton.interactable = false;
        selected = null;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;

        StopCoroutine(coroutine);
        coroutine = null;
    }

    private IEnumerator Coroutine()
    {
        yield return null;
        for (; ; )
        {
            if (Input.GetButtonDown("Inventory"))
            {
                Resume();
                yield break;
            }
            yield return null;
        }
    }

    private void UpdateUI(Item i, Item j)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        Inventory inventory = GameManager.instance.player1Stats.inventory;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.itemsCount)
            {
                Item item = inventory.GetItem(i);
                inventorySlots[i].AddItem(item);
            }
            else
            {
                inventorySlots[i].Clear();
            }
        }
        Equipment equipment = GameManager.instance.player1Stats.equipment;
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            Item item = equipment.GetItem(i);
            if (item != null)
            {
                equipmentSlots[i].AddItem(item);
            }
            else
            {
                equipmentSlots[i].Clear();
            }
        }
    }

    public void SelectItem(InventoryUISlot itemSlot, bool equipped)
    {
        selected = itemSlot;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] != itemSlot)
            {
                inventorySlots[i].Deselect();
            }
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i] != itemSlot)
            {
                equipmentSlots[i].Deselect();
            }
        }
        removeButton.interactable = true;
        equipButton.interactable = !equipped;
    }

    public void RemoveSelectedItem()
    {
        Inventory inventory = GameManager.instance.player1Stats.inventory;
        Equipment equipment = GameManager.instance.player1Stats.equipment;


        Item item = selected.item;
        bool wasEquiopment = selected.isEquipment;
        selected.Clear();
        selected = null;
        if (wasEquiopment)
        {
            GameManager.instance.SetupEquippedItem(item.equipmentType, null);
            equipment.UnEquip(item.equipmentType);
        }
        else
        {
            inventory.Remove(item);
            item = null;
        }
        equipButton.interactable = false;
        removeButton.interactable = false;

    }

    public void EquipSelectedItem()
    {
        Inventory inventory = GameManager.instance.player1Stats.inventory;
        Equipment equipment = GameManager.instance.player1Stats.equipment;

        if (selected.isEquipment)
        {
            Debug.LogError("Item is already equipped", equipButton);
            return;
        }
        Item item = selected.item;
        selected.Clear();
        selected = null;
        equipButton.interactable = false;
        removeButton.interactable = false;
        inventory.Remove(item);
        equipment.Equip(item);

        GameManager.instance.SetupEquippedItem(item.equipmentType, item);
    }
}
