using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject somethingToSpawn = null;
    [SerializeField]
    private int spawnCounter = 10;
    [SerializeField]
    private float spawnCooldown = 2;
    private float lastSomethingSpawned;

    [SerializeField]
    private float activationRange = 15f;

    [SerializeField]
    private Transform spawnArea = null;

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.right * spawnArea.lossyScale.x * 0.5f, Color.blue);

        if (PlayerControl.player != null && Vector3.Distance(PlayerControl.player.transform.position, transform.position) < activationRange)
        {
            if (Time.time > lastSomethingSpawned + spawnCooldown && spawnCounter > 0)
            {
                lastSomethingSpawned = Time.time;

                Vector3 spawnPosition;
                if (spawnArea != null)
                {
                    // get random point in circle with radius of 1
                    Vector2 random = Random.insideUnitCircle * spawnArea.lossyScale.x * 0.5f;
                    spawnPosition = transform.position + new Vector3(random.x, 0f, random.y);
                }
                else
                {
                    spawnPosition = transform.position;
                }

                GameObject newBullet = GameObject.Instantiate(somethingToSpawn, spawnPosition, transform.rotation);
                spawnCounter--;
            }
        }
    }
}
