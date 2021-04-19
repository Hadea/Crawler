using UnityEditor;
using UnityEngine;

public class FastIK : MonoBehaviour
{
    public int chainLenght;

    public Transform target;
    public bool createNewTarget = true;
    public Vector3 offset;
    public Transform pole;

    [Header("Solver Parameters")]
    public int iterations = 10;

    public float delta = 0.001f;

    public bool hideTargetTransforms = true;

    protected Transform[] bones;
    protected Vector3[] positions;
    protected float[] lenghts;
    protected float totalLenght;

    protected Vector3[] startDirectionSucc;
    protected Quaternion[] startRotations;
    protected Quaternion startRotationTarget;
    protected Quaternion startRotationRoot;

    public Vector3 targetPos
    {
        get
        {
            return target.TransformPoint(offset);
        }
    }

    void Awake()
    {
        Initialize();
    }

    void LateUpdate()
    {
        ResolveIK();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Transform current = transform;
        for (int i = 0; i < chainLenght && current != null && current.parent != null; i++)
        {
            float distance = Vector3.Distance(current.position, current.parent.position);
            float scale = distance * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, distance, scale));
            Handles.color = Color.yellow;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);

            current = current.parent;
        }
    }
#endif
    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.Lerp(Color.black, Color.green, 0.5f);
        //Gizmos.DrawWireSphere(pole.position, 0.1f);
    }

    private void Initialize()
    {
        bones = new Transform[chainLenght + 1];
        positions = new Vector3[chainLenght + 1];
        lenghts = new float[chainLenght];
        totalLenght = 0f;

        startDirectionSucc = new Vector3[chainLenght + 1];
        startRotations = new Quaternion[chainLenght + 1];

        if (target == null || createNewTarget)
        {
            Vector3 tmpPos = Vector3.zero;
            Quaternion tmpRot = Quaternion.identity;
            if (target != null)
            {
                tmpPos = target.position;
                tmpRot = target.rotation;
            }
            target = new GameObject(string.Concat(gameObject.name, " Target")).transform;
            if (target != null)
            {
                target.SetPositionAndRotation(tmpPos, tmpRot);
            }
            else
            {
                target.position = transform.position;
                target.Translate(offset, Space.Self);
            }
            if (hideTargetTransforms)
            {
                target.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild;
            }
        }
        startRotationTarget = target.rotation;

        Transform current = transform;
        for (int i = bones.Length - 1; i >= 0; i--)
        {
            bones[i] = current;
            startRotations[i] = current.rotation;

            if (i == bones.Length - 1)
            {
                startDirectionSucc[i] = targetPos - current.position;
            }
            else
            {
                startDirectionSucc[i] = bones[i + 1].position - current.position;
                lenghts[i] = startDirectionSucc[i].magnitude;
                totalLenght += lenghts[i];
            }

            current = current.parent;
        }
    }

    private void ResolveIK()
    {
        if (target == null)
        {
            return;
        }

        if (lenghts.Length != chainLenght)
        {
            Initialize();
        }

        // get positions
        for (int i = 0; i < bones.Length; i++)
        {
            positions[i] = bones[i].position;
        }

        Quaternion rootRotation = (bones[0].parent != null) ? bones[0].parent.rotation : Quaternion.identity;
        Quaternion rootRotationDiff = rootRotation * Quaternion.Inverse(startRotationRoot);

        // calculation
        if ((targetPos - bones[0].position).sqrMagnitude >= totalLenght * totalLenght)
        {
            // just stretch it;
            Vector3 direction = (targetPos - positions[0]).normalized;
            // set everything after root
            for (int i = 1; i < positions.Length; i++)
            {
                positions[i] = positions[i - 1] + direction * lenghts[i - 1];
            }
        }
        else
        {
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                // back
                for (int i = positions.Length - 1; i > 0; i--)
                {
                    if (i == positions.Length - 1)
                    {
                        positions[i] = targetPos;
                    }
                    else
                    {
                        positions[i] = positions[i + 1] + (positions[i] - positions[i + 1]).normalized * lenghts[i];
                    }
                }

                // forward
                for (int i = 1; i < positions.Length - 1; i++)
                {
                    positions[i] = positions[i - 1] + (positions[i] - positions[i - 1]).normalized * lenghts[i - 1];
                }

                // close enought?
                if ((positions[positions.Length - 1] - targetPos).sqrMagnitude < delta * delta)
                {
                    break;
                }
            }
        }

        // move towards pole
        if (pole != null)
        {
            for (int i = 1; i < positions.Length - 1; i++)
            {
                Plane plane = new Plane(positions[i + 1] - positions[i - 1], positions[i - 1]);
                Vector3 projectedPole = plane.ClosestPointOnPlane(pole.position);
                Vector3 projectedBone = plane.ClosestPointOnPlane(positions[i]);
                float angle = Vector3.SignedAngle(projectedBone - positions[i - 1], projectedPole - positions[i - 1], plane.normal);
                positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (positions[i] - positions[i - 1]) + positions[i - 1];
            }
        }

        // set positions & rotations
        for (int i = 0; i < positions.Length; i++)
        {
            Quaternion rotation;
            if (i == positions.Length - 1)
            {
                rotation = target.rotation * Quaternion.Inverse(startRotationTarget) * startRotations[i];
            }
            else
            {
                rotation = Quaternion.FromToRotation(startDirectionSucc[i], positions[i + 1] - positions[i]) * startRotations[i];
            }
            bones[i].SetPositionAndRotation(positions[i], rotation);
        }
    }
}
