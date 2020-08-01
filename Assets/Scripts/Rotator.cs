using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private Vector3 axis;

    void Update()
    {
        transform.Rotate(axis, rotationSpeed * Time.deltaTime);
    }
}
