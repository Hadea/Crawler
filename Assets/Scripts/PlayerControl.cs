using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl player;

    public ParticleSystem chargeEffect;

    public Transform meeleHand = null;
    public Transform rangedHand = null;
    private MeeleWeapon meeleWeapon;
    private RangedWeapon rangedWeapon;

    private float lastFired;
    private float chargeStart;

    public LayerMask targetMask;
    public float attackRange = 1.3f;
    public Transform hitArea;

    private NavMeshAgent agent;
    private GameManager.PlayerStats stats;

    void Awake()
    {
        player = this;
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.enabled = false;
        rangedWeapon?.ToggleRenderers(false);

        stats = GameManager.instance.player1Stats;
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

        bool doAttack = false;
        if (meeleWeapon != null && Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, targetMask) && Vector3.Distance(raycastHit.transform.position, transform.position) < attackRange + 0.5f)
            {
                doAttack = true;
                meeleWeapon.Attack(hitArea);
            }
        }

        if (!doAttack)
        {
            Movement();
        }
        Rotation();

        if (rangedWeapon != null && stats.ammo > 0 && Time.time > lastFired + rangedWeapon.stats.cooldown)
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
                stats.ammo--;

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

    public void SpawnOrDeleteEquippedItem(Item.EquipmentTypes slotType, Item item)
    {
        Transform holder = null;
        switch (slotType)
        {
            case Item.EquipmentTypes.MeeleWeapon:
                holder = meeleHand;
                meeleWeapon = null;
                break;
            case Item.EquipmentTypes.RangedWeapon:
                holder = rangedHand;
                rangedWeapon = null;
                break;
            case Item.EquipmentTypes.Armor:
                throw new System.NotImplementedException();
                //break;
        }

        for (int i = 0; i < holder.childCount; i++)
        {
            GameObject child = holder.GetChild(i).gameObject;
            Destroy(child);
        }

        if (item == null)
        {
            return;
        }
        GameObject instance = Instantiate(item.prefab, holder);
        instance.SetLayerRecursivly(LayerMask.NameToLayer("Player"));

        switch (slotType)
        {
            case Item.EquipmentTypes.MeeleWeapon:
                meeleWeapon = instance.GetComponent<MeeleWeapon>();
                break;
            case Item.EquipmentTypes.RangedWeapon:
                rangedWeapon = instance.GetComponent<RangedWeapon>();
                break;
            case Item.EquipmentTypes.Armor:
                throw new System.NotImplementedException();
                //break;
        }
    }
}
