using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Looting : MonoBehaviour
{
    [SerializeField]
    private string[] lootTags;

    public int score;

    private void OnTriggerEnter(Collider other)
    {
        // Check if object is loot
        foreach (var tag in lootTags)
        {
            if (other.gameObject.tag == tag)
            {
                CollectLoot(other.gameObject);
            }
        }
    }

    private void CollectLoot(GameObject loot)
    {
        // do something with loot depending on type
        switch (loot.tag)
        {
            case "Coin":
                CollectCoin();
                break;
            default:
                break;
        }
        // destroy loot object in game world
        Destroy(loot);
    }

    private void CollectCoin()
    {
        // increase score
        score++;
    }
}
