using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControl : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float aggroRange = 15f;
    public LayerMask layerMaskForPlayerSearch;
    public IEnemyAttackComponent enemyAttackComponent;
    private NavMeshAgent agent;
    private bool hasAggro;
    private Vector3 startPos;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        agent.enabled = true;
        enemyAttackComponent = GetComponent<IEnemyAttackComponent>();
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
            agent.SetDestination(PlayerControl.player.transform.position);
            agent.isStopped = false;
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
                if (Physics.Raycast(ray, out hit, 100f, layerMaskForPlayerSearch, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.GetComponent<PlayerControl>())
                    {
                        enemyAttackComponent.Attack(ray, distance);
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
}
