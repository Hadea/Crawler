using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField]
    //[ReadOnly]
    private List<Item> items = new List<Item>();
    public int itemsCount { get { return items.Count; } }
    public int space = 20;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChanged;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        onItemChanged?.Invoke();
    }

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
