using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Looting : MonoBehaviour
{
    private List<Lootable> alreadyLooting;

    void Start()
    {
        alreadyLooting = new List<Lootable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Lootable loot = other.GetComponent<Lootable>();

        if (loot != null && !alreadyLooting.Contains(loot))
        {
            alreadyLooting.Add(loot);
            StartCoroutine(SuckInItem(loot));
        }
    }

    private void CollectItem(Lootable loot)
    {
        if (loot != null)
        {
            switch (loot.lootType)
            {
                case Lootable.LootTypes.Item:
                    GameManager.instance.player1Stats.inventory.Add(loot.itemReference);
                    break;
                case Lootable.LootTypes.Coin:
                    GameManager.instance.player1Stats.coins += loot.amount;
                    break;
                case Lootable.LootTypes.Arrow:
                    GameManager.instance.player1Stats.ammo += loot.amount;
                    break;
            }
            alreadyLooting.Remove(loot);
            Destroy(loot.gameObject);
        }
        else
        {
            Debug.LogError("Loot item could not be handled", loot);
        }
    }

    private IEnumerator SuckInItem(Lootable loot)
    {
        float size = 0.55f;
        float sqrStartDistance = (transform.position - loot.transform.position).sqrMagnitude;
        float speed = 0f;
        Vector3 vector;
        do
        {
            vector = transform.position - loot.transform.position;
            speed = Mathf.Max(sqrStartDistance / (vector.sqrMagnitude) + 0.1f, speed);
            loot.transform.position = Vector3.MoveTowards(loot.transform.position, transform.position, speed * Time.deltaTime);
            yield return null;

        } while (vector.magnitude > size);

        CollectItem(loot);
    }
}
