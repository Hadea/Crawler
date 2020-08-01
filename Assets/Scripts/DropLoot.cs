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
    private LootDropChance[] loot = new LootDropChance[0];

    public void Drop()
    {
        GameObject loot = GetLootByChance();
        if (loot != null)
        {
            // spawn loot on death
            GameObject.Instantiate(loot, transform.position, transform.rotation);
        }
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
        // check if random is smaller than our current chance
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
