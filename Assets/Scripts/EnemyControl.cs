using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControl : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float aggroRange = 15f;
    public LayerMask layerMaskForPlayerSearch;
    public EnemyRangedAttack enemyAttackComponent;
    private NavMeshAgent agent;
    private bool hasAggro;
    private Vector3 startPos;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        agent.enabled = true;
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

        float distance = Vector3.Distance(transform.position, PlayerControl.player.transform.position);
        if (distance < aggroRange)
        {
            hasAggro = true;
        }
        else
        {
            hasAggro = false;
            agent.SetDestination(startPos);
            agent.isStopped = false;
        }

        if (hasAggro)
        {
            Ray ray = new Ray(transform.position + Vector3.up, PlayerControl.player.transform.position - transform.position);
            if (agent.hasPath ? agent.remainingDistance < aggroRange : true)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, layerMaskForPlayerSearch))
                {
                    if (hit.transform.GetComponent<PlayerControl>())
                    {

                        Quaternion lookRotation = Quaternion.LookRotation(hit.transform.position - transform.position);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                        agent.isStopped = true;
                        if (Vector3.Distance(transform.position + transform.forward * distance, PlayerControl.player.transform.position) < 0.45f)
                        {
                            Attack();
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
                        agent.SetDestination(PlayerControl.player.transform.position);
                        agent.isStopped = false;
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.gray;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, aggroRange);
#endif
    }

    private void Attack()
    {
    }
}
