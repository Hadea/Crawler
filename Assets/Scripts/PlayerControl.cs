using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl player;

    [SerializeField]
    private GameObject Bullet = null;
    [SerializeField]
    private Transform MuzzlePosition = null;

    [SerializeField]
    private float BulletSpeed = 30f;
    [SerializeField]
    private float BulletCooldown = 0.2f;
    private float lastFired;

    [SerializeField]
    private Animation HitAnimator = null;
    [SerializeField]
    private Transform HitArea = null;

    private NavMeshAgent agent;

    void Awake()
    {
        player = this;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Time.frameCount == 2)
        {
            agent.enabled = true;
            return;
        }
        Movement();
        Rotation();
        /*
        if (Input.GetButton("Fire1") && !HitAnimator.isPlaying)
        {
            Attack();
        }
        */
        if (Input.GetButton("Fire2") && Time.time > lastFired + BulletCooldown)
        {
            Shoot();
        }

    }

    private void OnDestroy()
    {
        if (UIManager.manager != null)
        {
            UIManager.manager.TogglePauseUI();
        }
    }

    private void Movement()
    {
        Vector3 movement = new Vector3();
        movement.x = agent.speed * Input.GetAxis("Horizontal");
        movement.z = agent.speed * Input.GetAxis("Vertical");

        if (movement.sqrMagnitude > 0.01f)
        {
            agent.isStopped = true;
            agent.Move(movement * Time.deltaTime);
        }
        else if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            float hit;
            if (plane.Raycast(ray, out hit))
            {
                Vector3 target = ray.origin + ray.direction * hit;
                agent.SetDestination(target);
                agent.isStopped = false;
            }
        }
    }

    private void Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up);

        float hit;
        Vector3 target = new Vector3();
        if (plane.Raycast(ray, out hit))
        {
            target = ray.origin + ray.direction * hit;
        }

        transform.rotation = Quaternion.LookRotation((target - transform.position).ToWithY(0f).normalized, Vector3.up);
    }

    private void Shoot()
    {
        lastFired = Time.time;
        GameObject newBullet = GameObject.Instantiate(Bullet, MuzzlePosition.position, transform.rotation);
        newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * BulletSpeed);
    }

    private void Attack()
    {
        HitAnimator.Play();
        Collider[] colliders = Physics.OverlapBox(HitArea.transform.position, HitArea.transform.localScale * 0.5f, HitArea.transform.rotation);
        foreach (var collider in colliders)
        {
            if (!collider.isTrigger)
            {
                OnBoxEnter(collider);
            }
        }
    }

    private void OnBoxEnter(Collider other)
    {
        // check if something with health has been hit
        Health health = other.transform.GetComponent<Health>();
        if (health != null)
        {
            // deal damage to target
            health.TakeDamage();
        }
    }
}
