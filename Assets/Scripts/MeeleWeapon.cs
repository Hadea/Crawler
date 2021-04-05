using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleWeapon : Weapon
{
    public MeeleWeaponStats stats;
    public Animation anim;

    protected override void Awake()
    {
        base.Awake();
        if (anim == null)
        {
            anim = GetComponentInChildren<Animation>();
        }
    }

    public void Attack(Transform hitArea)
    {
        if (anim.isPlaying)
        {
            return;
        }
        anim.Play();
        Collider[] colliders = Physics.OverlapBox(hitArea.transform.position, hitArea.transform.localScale * 0.5f, hitArea.transform.rotation, new LayerMask().ToEverything(), QueryTriggerInteraction.Ignore);
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
            health.TakeDamage(stats.dmg);
        }
    }
}
