using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl player;

    [SerializeField]
    private Transform muzzlePosition = null;
    private ParticleSystem chargeEffect;

    [SerializeField]
    private RangedWeapon rangedWeapon = null;
    public int ammo = 50;

    private float lastFired;
    private float chargeStart;

    [SerializeField]
    private Animation hitAnimator = null;
    [SerializeField]
    private Transform hitArea = null;

    private NavMeshAgent agent;

    void Awake()
    {
        player = this;
        agent = GetComponent<NavMeshAgent>();
        chargeEffect = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        agent.enabled = false;
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
        if (rangedWeapon != null && ammo > 0 && Time.time > lastFired + rangedWeapon.cooldown)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                chargeStart = Time.time;
                chargeEffect?.Play(true);
            }
            if (Input.GetButtonUp("Fire2"))
            {
                lastFired = Time.time;
                rangedWeapon.Shoot(muzzlePosition.position, muzzlePosition.rotation, Time.time - chargeStart > rangedWeapon.chargeTime);
                chargeEffect?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ammo--;
            }
        }
    }

    void OnDestroy()
    {
        if (UIManager.manager != null)
        {
            UIManager.manager.TogglePauseUI();
        }
    }

    private void Movement()
    {
        if (!agent.enabled)
        {
            return;
        }

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

    

    private void Attack()
    {
        hitAnimator.Play();
        Collider[] colliders = Physics.OverlapBox(hitArea.transform.position, hitArea.transform.localScale * 0.5f, hitArea.transform.rotation);
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
            health.TakeDamage(1);
        }
    }
}
