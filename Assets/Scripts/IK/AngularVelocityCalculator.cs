using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularVelocityCalculator : MonoBehaviour
{
    [Flags]
    public enum UpdateInterval : int
    {
        Update = 1,
        LateUpdate = 2,
        FixedUpdate = 4,
    }
    public UpdateInterval updatingInterval = UpdateInterval.Update | UpdateInterval.LateUpdate;

    [SerializeField]
    [ReadOnly]
    private Vector3 vel;
    public Vector3 velocity
    {
        get
        {
            return vel;
        }
    }
    [SerializeField]
    [ReadOnly]
    private Vector3 angularVel;
    public Vector3 angularVelocity
    {
        get
        {
            return angularVel;
        }
    }

    private Vector3 lastPos;
    private Quaternion lastRot;
    private float lastTime = float.NaN;

    void Update()
    {
        if (Includes(updatingInterval, UpdateInterval.Update))
        {
            Calculate();
        }
    }

    void LateUpdate()
    {
        if (Includes(updatingInterval, UpdateInterval.LateUpdate))
        {
            Calculate();
        }
    }

    void FixedUpdate()
    {
        if (Includes(updatingInterval, UpdateInterval.FixedUpdate))
        {
            Calculate();
        }
    }

    public static bool Includes(UpdateInterval enumValue, UpdateInterval layer)
    {
        return ((int)enumValue & (1 << (int)layer)) != 0;
    }

    public void Calculate()
    {
        if (!float.IsNaN(lastTime))
        {
            vel = (transform.position - lastPos) / (Time.time - lastTime);
            angularVel = Quaternion_Extension.ToAngularVelocity(lastRot, transform.rotation, Time.time - lastTime);
        }

        lastPos = transform.position;
        lastRot = transform.rotation;
        lastTime = Time.time;
    }
}
