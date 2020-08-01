using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControl : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private float aggroRange = 15f;

    [SerializeField]
    private LayerMask layerMaskForPlayerSearch = new LayerMask();

    [SerializeField]
    private float rotationSpeed = 20f;

    [SerializeField]
    private GameObject Bullet = null;
    [SerializeField]
    private Transform MuzzlePosition = null;

    [SerializeField]
    private float BulletSpeed = 30f;
    [SerializeField]
    private float BulletCooldown = 1f;
    private float lastBulletFired;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check if player is in aggro range
        if (PlayerControl.player != null && Vector3.Distance(PlayerControl.player.transform.position, transform.position) < aggroRange)
        {
            // Check if player is visible from own position
            Ray ray = new Ray(transform.position + Vector3.up, PlayerControl.player.transform.position - transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, layerMaskForPlayerSearch))
            {
                // If player is found, start shooting
                if (hit.transform.GetComponent<PlayerControl>())
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.green);

                    // rotation for looking towards the player
                    Quaternion lookRotation = Quaternion.LookRotation(hit.transform.position - transform.position);

                    // slowly rotate towards player
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                    // stop movement
                    navMeshAgent.isStopped = true;
                    if (Vector3.Distance(transform.position + transform.forward * hit.distance, hit.point) < 1f)
                    {
                        Shoot();
                    }
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    // Else, walk towards the player
                    navMeshAgent.SetDestination(PlayerControl.player.transform.position);
                    navMeshAgent.isStopped = false;
                }
            }
        }
    }

    private void Shoot()
    {
        if (Time.time > lastBulletFired + BulletCooldown)
        {
            lastBulletFired = Time.time;
            GameObject newBullet = GameObject.Instantiate(Bullet, MuzzlePosition.position, transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * BulletSpeed);
        }
    }
}
