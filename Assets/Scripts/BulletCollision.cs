using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public float speed { set; private get; }
    public GameObject hitEffect;
    public float raycastLenght = 0.3f;
    public LayerMask raycastMask;
    public bool canGetStuck;
    public float stuckTime = 5f;
    public int damage { set; private get; }

    void Start()
    {
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        Ray ray = transform.CreateForwardRay();
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastLenght, raycastMask, QueryTriggerInteraction.Ignore))
        {
            OnHit(hit.transform);
        }
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward * raycastLenght, Color.white);
    }

    private void OnHit(Transform other)
    {

        // check if something with health has been hit
        Health health = other.root.GetComponentInChildren<Health>();
        if (health != null)
        {
            // deal damage to target
            health.TakeDamage(damage);
        }
        // spawn hit effect on current position
        if (hitEffect != null)
        {
            GameObject.Instantiate(hitEffect, transform.position - transform.forward * raycastLenght, transform.rotation * Quaternion.Euler(Vector3.up * 180f));
        }
        if (health != null)
        {
            // destroy bullet
            Destroy(gameObject);
            return;
        }
        if (canGetStuck && other.gameObject.isStatic)
        {
            gameObject.AddComponent<DestroyAfterTime>().timeToLive = stuckTime;
            transform.position += -transform.forward * raycastLenght;
            Destroy(GetComponent<Collider>());
            Destroy(GetComponentInChildren<Rotator>());
            Destroy(this);
            speed = 0f;
        }
    }
}
