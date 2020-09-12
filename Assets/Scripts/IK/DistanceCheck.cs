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
    }

    public LegStep[] legSteps;
    public float stepThreshhold = 0.75f;

    public AnimationCurve stepCurve;
    public float stepDuration = 1f;

    private void Start()
    {
        StartCoroutine(ZigZagSetup());
    }

    void Update()
    {
        for (int i = 0; i < legSteps.Length; i++)
        {
            int j = (i + 2) % legSteps.Length;
            Vector3 vector = legSteps[i].bodyTarget.position - legSteps[i].leg.target.position;
            if (vector.sqrMagnitude > stepThreshhold * stepThreshhold && !legSteps[i].stepping && !legSteps[j].stepping)
            {
                legSteps[i].stepping = true;
                Vector3 originalVector = vector.normalized;
                //Debug.DrawRay(legSteps[i].bodyTarget.position, originalVector * stepThreshhold, Color.blue, .5f);
                vector = legSteps[i].bodyTarget.InverseTransformVector(vector.normalized);
                vector = vector.ToWithX(0f).normalized;
                vector = legSteps[i].bodyTarget.TransformVector(vector);
                Vector3 correctedVector = vector.normalized;
                //Debug.DrawRay(legSteps[i].bodyTarget.position, correctedVector * stepThreshhold, Color.magenta, .5f);
                vector = Vector3.Lerp(originalVector, correctedVector, 0.5f).normalized;
                //Debug.DrawRay(legSteps[i].bodyTarget.position, vector * stepThreshhold, Color.red, .5f);
                StartCoroutine(StepAnim(legSteps[i], vector * stepThreshhold));
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < legSteps.Length; i++)
        {
            Handles.color = Color.red;
            Handles.DrawLine(legSteps[i].bodyTarget.position, legSteps[i].leg.target.position);
            Handles.color = Color.blue;
            Handles.DrawWireDisc(legSteps[i].bodyTarget.position, legSteps[i].bodyTarget.up, stepThreshhold);
        }
    }

    private IEnumerator StepAnim(LegStep step, Vector3 vector)
    {
        Vector3 startPos = step.leg.target.position;
        Vector3 endPos = step.bodyTarget.position + vector;
        for (float duration = 0f; duration < stepDuration * 0.9f; duration += Time.deltaTime)
        {
            float percentage = stepDuration * 0.5f != 0f ? duration / (stepDuration * 0.5f) : 0f;

            step.leg.target.position = Vector3.Lerp(startPos, endPos, stepCurve.Evaluate(percentage));

            yield return null;
        }
        yield return Yielders.Get(stepDuration * 0.1f);
        step.stepping = false;
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
