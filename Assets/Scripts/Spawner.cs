using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject somethingToSpawn;
    [SerializeField]
    private int spawnCounter = 10;
    [SerializeField]
    private float spawnCooldown = 2;
    private float lastSomethingSpawned;

    [SerializeField]
    private float activationRange = 15f;

    void Update()
    {
        if (PlayerControl.player != null && Vector3.Distance(PlayerControl.player.transform.position, transform.position) < activationRange)
        {
            if (Time.time > lastSomethingSpawned + spawnCooldown && spawnCounter > 0)
            {
                lastSomethingSpawned = Time.time;
                GameObject newBullet = GameObject.Instantiate(somethingToSpawn);
                newBullet.transform.position = transform.position;
                newBullet.transform.rotation = transform.rotation;
                spawnCounter--;
            }
        }
    }
}
