using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float MovementSpeed;


    void Update()
    {
        Vector3 movement = new Vector3();
        movement.x = MovementSpeed * Input.GetAxis("Horizontal");
        movement.z = MovementSpeed * Input.GetAxis("Vertical");

        transform.Translate(movement * Time.deltaTime, Space.World);
    }
}
