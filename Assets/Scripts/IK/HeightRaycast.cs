using UnityEngine;

public class HeightRaycast : MonoBehaviour
{
    public Transform[] transforms;
    public LayerMask mask = new LayerMask().ToEverything();
    public float maxDistance = 2f;

    void Update()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            Transform child = transforms[i];

            Ray ray = new Ray(transform.TransformPoint(transform.InverseTransformPoint(child.position).ToWithY(0)), -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
            {
                child.position = hit.point;
                //child.up = hit.normal;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            Transform child = transforms[i];

            //Debug.DrawRay(transform.TransformPoint(transform.InverseTransformPoint(child.position).ToWithY(0)), -transform.up * maxDistance, Color.cyan);
        }
    }
}
