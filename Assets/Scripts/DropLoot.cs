using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    [System.Serializable]
    public struct LootDropChance
    {
        public GameObject lootObject;
        public int dropChance;
    }

    [SerializeField]
    private LootDropChance[] loot;

    private void OnDestroy()
    {
        Debug.Log("drop loot: " + GetLootByChance());
    }

    private GameObject GetLootByChance()
    {
        // calculate total of all chances
        int total = 0;
        foreach (var chance in loot)
        {
            total += chance.dropChance;
        }

        int random = Random.Range(0, total);
        total = 0;
        foreach (var chance in loot)
        {
            if (random <= total)
            {
                return chance.lootObject;
            }
            total += chance.dropChance;
        }
        throw new System.Exception("No loot found");
    }
}
