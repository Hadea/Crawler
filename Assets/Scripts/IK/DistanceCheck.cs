using System.Collections;
using UnityEditor;
using UnityEngine;

public class DistanceCheck : MonoBehaviour
{
    [System.Serializable]
    public class LegStep
    {
        public FastIK leg;
        public Transform bodyTarget;
        public bool stepping;
        public Vector3? point;
    }

    public LegStep[] legSteps;
    public float stepThreshhold = 0.75f;

    public float stepDuration = 1f;
    public float waitDuration = 0f;
    public AnimationCurve stepLerpCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    public float stepHeight = 0.5f;
    public AnimationCurve stepHeightCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));

    [Space]
    [SerializeField]
    [ReadOnly]
    private Vector3 velocity;
    [SerializeField]
    [ReadOnly]
    private Vector3 angularVelocity;
    [Space]
    [SerializeField]
    [ReadOnly]
    private Vector3 linear;
    [SerializeField]
    [ReadOnly]
    private Vector3 angular;

    void Start()
    {
        StartCoroutine(ZigZagSetup());
    }

    void Update()
    {
        velocity = GetComponentInParent<AngularVelocityCalculator>().velocity;
        angularVelocity = GetComponentInParent<AngularVelocityCalculator>().angularVelocity;

        bool[] doStep = new bool[legSteps.Length];
        for (int i = 0; i < 2; i++)
        {
            Vector3 distance = legSteps[i].bodyTarget.position - legSteps[i].leg.target.position;
            if (distance.sqrMagnitude > stepThreshhold * stepThreshhold && !legSteps[i].stepping)
            {
                bool othersStepping = false;
                bool thisEven = i % 4 == 0 || i % 4 == 3;
                for (int j = 0; j < legSteps.Length; j++)
                {
                    bool otherUneven = j % 4 == 1 || j % 4 == 2;
                    if (thisEven && otherUneven || !thisEven && !otherUneven)
                    {
                        othersStepping = othersStepping || legSteps[j].stepping || doStep[j];
                        if (othersStepping)
                        {
                            break;
                        }
                    }
                }
                if (!othersStepping)
                {
                    for (int j = 0; j < legSteps.Length; j++)
                    {
                        bool otherEven = j % 4 == 0 || j % 4 == 3;
                        if (thisEven && otherEven || !thisEven && !otherEven)
                        {
                            doStep[j] = true;//Vector3.Distance(legSteps[j].bodyTarget.position, legSteps[j].leg.target.position) / stepThreshhold > 0.3f;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < legSteps.Length; i++)
        {
            if (doStep[i] && !legSteps[i].stepping)
            {
                legSteps[i].stepping = true;
                Vector3 vector = Vector3.zero;
                /*
                vector = legSteps[i].bodyTarget.position - legSteps[i].leg.target.position;
                Vector3 originalVector = vector.normalized;
                //Debug.DrawRay(legSteps[i].bodyTarget.position, originalVector * stepThreshhold, Color.blue);
                vector = legSteps[i].bodyTarget.InverseTransformVector(vector.normalized);
                vector = vector.ToWithX(0f).normalized;
                vector = legSteps[i].bodyTarget.TransformVector(vector);
                Vector3 correctedVector = vector.normalized;
                //Debug.DrawRay(legSteps[i].bodyTarget.position, correctedVector * stepThreshhold, Color.magenta);
                vector = Vector3.Lerp(originalVector, correctedVector, 0f).normalized;
                //Debug.DrawRay(legSteps[i].bodyTarget.position, vector * stepThreshhold, Color.red);
                */
                linear = Vector3.zero;
                if (velocity.sqrMagnitude > 0f)
                {
                    linear = velocity.normalized;
                }
                angular = Vector3.zero;
                if (Mathf.Abs(angularVelocity.y) > 0f)
                {
                    Vector3 tmpDir = (legSteps[i].bodyTarget.position - transform.position).ToWithY(0f).normalized;
                    Vector3 l = Vector3.Cross(tmpDir, legSteps[i].bodyTarget.up);
                    Debug.DrawRay(legSteps[i].bodyTarget.position, l, Color.blue);
                    Vector3 r = Vector3.Cross(legSteps[i].bodyTarget.up, tmpDir);
                    Debug.DrawRay(legSteps[i].bodyTarget.position, r, Color.red);
                    if (angularVelocity.y.SignOr0() != 0f)
                    {
                        angular = angularVelocity.y.Sign() < 0f ? l : r;
                    }
                }
                float balance = 0.5f;
                if (linear.magnitude > 0f)
                {
                    balance -= 0.5f;
                }
                if (angular.magnitude > 0f)
                {
                    balance += 0.5f;
                }
                vector = Vector3.Lerp(linear, angular, balance);
                StartCoroutine(StepAnim(legSteps[i], vector * stepThreshhold));
            }
        }
        if (velocity.sqrMagnitude == 0f && angularVelocity.y == 0f)
        {
            linear = Vector3.zero;
            angular = Vector3.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < legSteps.Length; i++)
        {
            if (legSteps[i].bodyTarget != null && legSteps[i].leg)
            {
                Handles.color = Color.Lerp(Color.yellow, Color.red, 0.6f);
                Handles.DrawLine(legSteps[i].bodyTarget.position, legSteps[i].leg.target.position);
                Handles.color = Color.cyan;
                Handles.DrawWireDisc(legSteps[i].bodyTarget.position, legSteps[i].bodyTarget.up, stepThreshhold);

                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(legSteps[i].leg.target.position, 0.1f);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(legSteps[i].bodyTarget.position, 0.1f);

                if (legSteps[i].point.HasValue)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawWireSphere(legSteps[i].point.Value, 0.1f);
                }
            }
        }
    }

    private IEnumerator StepAnim(LegStep step, Vector3 vector)
    {
        Vector3 startPos = step.leg.target.position;
        Vector3 endPos = step.bodyTarget.position + vector * (stepDuration == 0f ? 0.99f : 1f);
        step.point = endPos;
        for (float duration = 0f; duration < stepDuration; duration += Time.deltaTime)
        {
            float percentage = stepDuration * 0.5f != 0f ? duration / (stepDuration * 0.5f) : 0f;

            step.leg.target.position = Vector3.Lerp(startPos, endPos, stepLerpCurve.Evaluate(percentage)) + step.bodyTarget.up * stepHeightCurve.Evaluate(percentage) * stepHeight;

            yield return null;
        }
        if (waitDuration > 0f)
        {
            yield return Yielders.Get(waitDuration);
        }
        step.stepping = false;
        step.point = null;
    }

    private IEnumerator ZigZagSetup()
    {
        Vector3[] tmpPositions = new Vector3[legSteps.Length];

        for (int i = 0; i < legSteps.Length; i++)
        {
            tmpPositions[i] = legSteps[i].bodyTarget.position;

            legSteps[i].bodyTarget.Translate(Vector3.back * stepThreshhold * 0.5f * (i % 4 == 1 || i % 4 == 2 ? -1f : 1f), Space.Self);
            legSteps[i].leg.target.position = legSteps[i].bodyTarget.position;
        }
        yield return null;
        for (int i = 0; i < legSteps.Length; i++)
        {
            legSteps[i].bodyTarget.position = tmpPositions[i];
        }
    }
}
