using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject hitEffect = null;
    [SerializeField]
    private float offset = 0.04f;
    [SerializeField]
    private bool canGetStuck = false;
    [SerializeField]
    private float stuckTime = 5f;
    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ignore other projectiles
        if (other.transform.GetComponent<BulletCollision>())
        {
            return;
        }

        // check if something with health has been hit
        Health health = other.transform.GetComponent<Health>();
        if (health != null)
        {
            // deal damage to target
            health.TakeDamage();
        }
        // spawn hit effect on current position
        if (hitEffect != null)
        {
            float velocity = rigid.velocity.magnitude;
            GameObject.Instantiate(hitEffect, transform.position - transform.forward * velocity * offset, transform.rotation * Quaternion.Euler(Vector3.up * 180f));
        }
        if (health != null)
        {
            // destroy bullet
            Destroy(gameObject);
            return;
        }
        if (canGetStuck)
        {
            rigid.velocity = Vector3.zero;
            gameObject.AddComponent<DestroyAfterTime>().timeToLive = stuckTime;
            transform.position += -transform.forward * offset;
            Destroy(GetComponent<Collider>());
            Destroy(GetComponentInChildren<Rotator>());
            Destroy(rigid);
            Destroy(this);
        }
    }
}
