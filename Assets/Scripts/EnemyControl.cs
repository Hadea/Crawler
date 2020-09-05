using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControl : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]
    private float aggroRange = 15f;

    [SerializeField]
    private LayerMask layerMaskForPlayerSearch = new LayerMask();

    [SerializeField]
    private float rotationSpeed = 90f;

    [SerializeField]
    private int bulletDamage = 1;
    [SerializeField]
    private GameObject bullet = null;
    [SerializeField]
    private Transform muzzlePosition = null;

    [SerializeField]
    private float bulletSpeed = 30f;
    [SerializeField]
    private float bulletCooldown = 1f;
    private float lastBulletFired;

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

        // Check if player is in aggro range
        float distance = Vector3.Distance(PlayerControl.player.transform.position, transform.position);
        if (distance < aggroRange && (agent.hasPath ? agent.remainingDistance < aggroRange : true))
        {
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
        if (Time.time > lastBulletFired + bulletCooldown)
        {
            lastBulletFired = Time.time;
            GameObject newBullet = GameObject.Instantiate(bullet, muzzlePosition.position, transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        newBullet.GetComponent<BulletCollision>().damage = bulletDamage;
        }
    }
}
