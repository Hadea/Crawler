using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl player;

    [SerializeField]
    private float MovementSpeed;

    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    private Transform MuzzlePosition;

    [SerializeField]
    private float BulletSpeed;
    [SerializeField]
    private float BulletCooldown;
    private float lastBulletFired;

    void Start()
    {
        player = this;
    }

    void Update()
    {

        #region Movement

        Vector3 movement = new Vector3();
        movement.x = MovementSpeed * Input.GetAxis("Horizontal");
        movement.z = MovementSpeed * Input.GetAxis("Vertical");

        transform.Translate(movement * Time.deltaTime, Space.World);
        #endregion

        #region Rotation

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        float hit;
        Vector3 target = new Vector3();
        if (plane.Raycast(ray, out hit))
        {
            target = ray.origin + ray.direction * hit;
        }

        transform.LookAt(target);
        #endregion

        #region Shoot

        if (Input.GetButton("Fire1") && Time.time > lastBulletFired + BulletCooldown )
        {
            lastBulletFired = Time.time;
            GameObject newBullet = GameObject.Instantiate(Bullet);
            newBullet.transform.position = MuzzlePosition.position;
            newBullet.transform.rotation = transform.rotation;
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * BulletSpeed);
        }

        #endregion

    }
}
