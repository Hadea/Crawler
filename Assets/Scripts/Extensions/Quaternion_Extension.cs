using UnityEngine;

public static class Quaternion_Extension
{
    public static Vector3 ToAngularVelocity(this Quaternion start, Quaternion end, float delta)
    {
        Quaternion deltaRot = end * Quaternion.Inverse(start);
        Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y), Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));

        return eulerRot / delta;
    }
}
