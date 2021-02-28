using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Coroutine coroutine;

    public Transform slotsParent;
    private InventoryUISlot[] slots;

    public InventoryUISlot selected { get; private set; }

    public Button removeButton;

    void Awake()
    {
        gameObject.SetActive(false);

        slots = slotsParent.GetComponentsInChildren<InventoryUISlot>();

        removeButton.interactable = false;
    }

    void OnEnable()
    {
        Time.timeScale = 0f;

        coroutine = StartCoroutine(Coroutine());
    }

    void OnDisable()
    {
        removeButton.interactable = false;
        selected = null;
    }

    public void Init()
    {
        selected = null;
        GameManager.instance.player1Stats.inventory.onItemChanged += UpdateUI;
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

    private void UpdateUI()
    {
        Inventory inventory = GameManager.instance.player1Stats.inventory;
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.itemsCount)
            {
                slots[i].AddItem(inventory.GetItem(i));
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    public void SelectItem(InventoryUISlot itemSlot)
    {
        selected = itemSlot;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != itemSlot)
            {
                slots[i].Deselect();
            }
        }
        removeButton.interactable = true;
    }

    public void RemoveSelectedItem()
    {
        GameManager.instance.player1Stats.inventory.Remove(selected.item);
        selected.Clear();
        selected = null;
        removeButton.interactable = false;
    }

    public void UseSelectedItem()
    {
        selected.item?.Use();
    }
}
