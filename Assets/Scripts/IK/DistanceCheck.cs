using System.Collections;
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
    public float stepSize = 0.75f * 1.2f;

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
            Vector3 vector2 = legSteps[j].bodyTarget.position - legSteps[j].leg.target.position;
            if (vector.sqrMagnitude > stepThreshhold * stepThreshhold && !legSteps[i].stepping && !legSteps[j].stepping && !legSteps[(i + 1) % legSteps.Length].stepping)
            {
                legSteps[i].stepping = true;
                StartCoroutine(StepAnim(legSteps[i], vector.normalized * Mathf.Max(stepSize, vector.magnitude)));
                StartCoroutine(StepAnim(legSteps[j], vector2.normalized * Mathf.Max(stepSize, vector2.magnitude)));
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Color color = Color.red;
        for (int i = 0; i < legSteps.Length; i++)
        {
            Debug.DrawLine(legSteps[i].bodyTarget.position, legSteps[i].leg.target.position, color);
        }
    }

    private IEnumerator StepAnim(LegStep step, Vector3 distance)
    {
        Vector3 startPos = step.leg.target.position;
        for (float duration = 0f; duration < stepDuration * 0.5f; duration += Time.deltaTime)
        {
            float percentage = stepDuration * 0.5f != 0f ? duration / (stepDuration * 0.5f) : 0f;

            step.leg.target.position = startPos + distance * stepCurve.Evaluate(percentage);

            yield return null;
        }
        yield return Yielders.Get(stepDuration * 0.5f);
        step.stepping = false;
    }

    private IEnumerator ZigZagSetup()
    {
        Vector3[] tmpPositions = new Vector3[legSteps.Length];

        for (int i = 0; i < legSteps.Length; i++)
        {
            tmpPositions[i] = legSteps[i].bodyTarget.position;
            if (i % 4 == 1 || i % 4 == 2)
            {
                legSteps[i].bodyTarget.Translate(Vector3.back * stepThreshhold * 0.5f, Space.Self);
                legSteps[i].leg.target.position = legSteps[i].bodyTarget.position;
            }
        }
        yield return null;
        for (int i = 0; i < legSteps.Length; i++)
        {
            legSteps[i].bodyTarget.position = tmpPositions[i];
        }
    }
}
