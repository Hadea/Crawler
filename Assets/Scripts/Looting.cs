using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Looting : MonoBehaviour
{
    [SerializeField]
    private string[] lootTags = new string[0];

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
                StartCoroutine(CollectCoin(loot));
                break;
        }
        // destroy loot object in game world
    }

    private IEnumerator CollectCoin(GameObject coin)
    {
        float size = 0.55f;
        float sqrStartDistance = (transform.position - coin.transform.position).sqrMagnitude;
        float speed = 0f;
        Vector3 vector;
        do
        {
            vector = transform.position - coin.transform.position;
            speed = Mathf.Max(sqrStartDistance / (vector.sqrMagnitude) + 0.1f, speed);
            coin.transform.position = Vector3.MoveTowards(coin.transform.position, transform.position, speed * Time.deltaTime);
            yield return null;

        } while (vector.magnitude > size);
        // increase score
        if (UIManager.instance != null)
        {
            UIManager.instance.score++;
        }
        Destroy(coin);
    }
}
