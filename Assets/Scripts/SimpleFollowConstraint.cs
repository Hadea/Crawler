using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SimpleFollowConstraint : MonoBehaviour
{
    public Transform followObject;
    public bool followRotation = true;
    public Vector3 offset;
    public Space offsetRelativeTo = Space.Self;

    void LateUpdate()
    {
        transform.position = followObject.position + (offsetRelativeTo == Space.Self ? followObject.TransformDirection(offset) : offset);
        if (followRotation)
        {
            transform.rotation = followObject.rotation;
        }
    }
}
