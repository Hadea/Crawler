using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    public enum LootTypes
    {
        Item,
        Coin,
        Arrow,
    }

    public LootTypes lootType;
    public int amount = 1;
    public Item itemReference;
}
