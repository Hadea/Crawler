using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl player;

    public ParticleSystem chargeEffect;

    public MeeleWeapon meeleWeapon;
    public RangedWeapon rangedWeapon;
    public int ammo = 50;

    private float lastFired;
    private float chargeStart;

    public Transform hitArea;

    private NavMeshAgent agent;

    void Awake()
    {
        player = this;
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.enabled = false;
        rangedWeapon.ToggleRenderers(false);
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
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
            meeleWeapon.Attack(hitArea);
        }
        */
        if (rangedWeapon != null && ammo > 0 && Time.time > lastFired + rangedWeapon.stats.cooldown)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                chargeStart = Time.time;
                chargeEffect?.Play(true);

                meeleWeapon.ToggleRenderers(false);
                rangedWeapon.ToggleRenderers(true);
            }
            if (Input.GetButtonUp("Fire2"))
            {
                lastFired = Time.time;
                rangedWeapon.Shoot(Time.time - chargeStart > rangedWeapon.stats.chargeTime);
                chargeEffect?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ammo--;

                meeleWeapon.ToggleRenderers(true);
                rangedWeapon.ToggleRenderers(false);
            }
        }
    }

    void OnDestroy()
    {
        if (UIManager.instance != null)
        {
            UIManager.instance.TogglePauseUI();
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






}
