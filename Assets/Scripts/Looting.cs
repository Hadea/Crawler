using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Looting : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Lootable loot = other.GetComponent<Lootable>();

        if (loot != null)
        {
            StartCoroutine(SuckInItem(loot));
        }
    }

    private void CollectItem(Lootable loot)
    {
        if (loot != null)
        {
            if (loot.isCoin)
            {
                GameManager.instance.player1Stats.coins++;
            }
            else
            {
                GameManager.instance.player1Stats.inventory.Add(loot.itemReference);
            }
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
