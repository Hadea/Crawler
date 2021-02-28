using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [SerializeField]
    private List<Item> items = new List<Item>();

    public int itemsCount { get { return items.Count; } }
    public int space = 20;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChanged;

    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            return false;
        }
        items.Add(item);
        onItemChanged?.Invoke();
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        onItemChanged?.Invoke();
    }

    public Item GetItem(int index)
    {
        return items[index];
    }
}
