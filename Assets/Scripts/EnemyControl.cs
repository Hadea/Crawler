using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControl : MonoBehaviour
{
    private NavMeshAgent agent;

    public float aggroRange = 15f;

    public LayerMask layerMaskForPlayerSearch;

    public float rotationSpeed = 90f;

    public int projectileDamage = 1;
    public GameObject projectilePrefab;
    public Transform muzzlePosition;

    public float projectileSpeed = 30f;
    public float projectileCooldown = 1f;
    private float lastBulletFired;

    private bool hasAggro;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    void Update()
    {
        if (PlayerControl.player == null)
        {
            return;
        }
        if (Time.frameCount >= 2)
        {
            agent.enabled = true;
        }

        // Check if player is in aggro range
        float distance = Vector3.Distance(PlayerControl.player.transform.position, transform.position);
        if ((distance < aggroRange || hasAggro) && (agent.hasPath ? agent.remainingDistance < aggroRange : true))
        {
            hasAggro = true;
            // Check if player is visible from own position
            Ray ray = new Ray(transform.position + Vector3.up, PlayerControl.player.transform.position - transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, layerMaskForPlayerSearch))
            {
                // If player is found, start shooting
                if (hit.transform.GetComponent<PlayerControl>())
                {

                    // rotation for looking towards the player
                    Quaternion lookRotation = Quaternion.LookRotation(hit.transform.position - transform.position);

                    // slowly rotate towards player
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                    // stop movement
                    agent.isStopped = true;
                    if (Vector3.Distance(transform.position + transform.forward * distance, PlayerControl.player.transform.position) < 0.45f)
                    {
                        Shoot();
                        Debug.DrawLine(ray.origin, hit.point, Color.green);
                    }
                    else
                    {
                        Debug.DrawLine(ray.origin, hit.point, Color.blue);
                    }
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    // otherwise, walk towards the player
                    agent.SetDestination(PlayerControl.player.transform.position);
                    agent.isStopped = false;
                }
            }
        }
    }

    private void Shoot()
    {
        if (Time.time > lastBulletFired + projectileCooldown)
        {
            lastBulletFired = Time.time;
            GameObject newBullet = Instantiate(projectilePrefab, muzzlePosition.position, muzzlePosition.rotation);
            BulletCollision bulletController = newBullet.GetComponent<BulletCollision>();
            bulletController.speed = projectileSpeed;
            bulletController.damage = projectileDamage;
        }
    }
}
