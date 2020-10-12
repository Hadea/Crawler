using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Coroutine coroutine;
    private Inventory inventory;

    public Transform slotsParent;
    private InventorySlot[] slots;

    private Item selected;

    public Button removeButton;

    void Awake()
    {
        gameObject.SetActive(false);
        inventory = Inventory.instance;
    }

    void Start()
    {
        inventory.onItemChanged += UpdateUI;
        slots = slotsParent.GetComponentsInChildren<InventorySlot>();
    }

    void OnEnable()
    {
        Time.timeScale = 0f;

        coroutine = StartCoroutine(Coroutine());

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

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.itemsCount)
            {
                slots[i].AddItem(inventory.GetItem(i));
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void SelectItem(Item item)
    {
        selected = item;
        removeButton.interactable = true;
    }

    public void RemoveSelectedItem()
    {
        inventory.Remove(selected);
        selected = null;
        removeButton.interactable = false;
    }

    public void UseSelectedItem()
    {
        selected?.Use();
    }
}
