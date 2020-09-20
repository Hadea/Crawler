using UnityEngine;
using UnityEngine.AI;

public class MovementTestController : MonoBehaviour
{
    public enum UseVelocityFrom
    {
        Rigidbody,
        NavMeshAgent,
    }

    public UseVelocityFrom velocityTo;
    public float rigidSpeedMult = 10f;
    public float rigidRotMult = 1f;

    public float agentSpeedMult = 1f;
    public float agentRotMult = 1f;

    private Rigidbody rigid;
    private NavMeshAgent agent;

    [SerializeField]
    [ReadOnly]
    private Vector3 velocity;
    [SerializeField]
    [ReadOnly]
    private Vector3 angularVelocity;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        velocity = new Vector3(x, 0f, z);

        angularVelocity = Vector3.up * Input.GetAxis("Turn");

        switch (velocityTo)
        {
            case UseVelocityFrom.Rigidbody:
                if (rigid != null)
                {
                    MoveWithRigid();
                }
                break;
            case UseVelocityFrom.NavMeshAgent:
                if (agent != null)
                {
                    MoveWithAgent();
                }
                break;
        }
    }


    private void MoveWithRigid()
    {
        rigid.AddRelativeForce(velocity * rigidSpeedMult, ForceMode.Acceleration);
        rigid.AddRelativeTorque(angularVelocity * rigidRotMult, ForceMode.Acceleration);
    }

    private void MoveWithAgent()
    {
        agent.Move(transform.TransformDirection(velocity) * agentSpeedMult * Time.deltaTime);
        transform.Rotate(angularVelocity * agentRotMult * agent.angularSpeed * Time.deltaTime, Space.Self);
    }
}
