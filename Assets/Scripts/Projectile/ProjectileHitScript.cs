using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The projectile damage script (handles all projectile damaging action)
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ProjectileHitScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private ProjectileScript projectileScript;

    // Components
    [SerializeField]
    private Collider2D projectileCollider; // To disable collider after hitting something
    internal GameObject attacker;

    // Variables
    private bool hit; // To prevent bullet from repeatedly registering consecutive hits

    // Start is called before the first frame update
    void Start()
    {
        projectileCollider = GetComponentInChildren<Collider2D>();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Set hit variable to false (bullet hasn't hit anything yet)
        hit = false;

        // Enable collider
        projectileCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (HasHit())
        {
            // If projectile has hit something, disable collider
            projectileCollider.enabled = false;
        }
    }

    internal void SetAttacker(GameObject attacker)
    {
        this.attacker = attacker;
    }

    // Check if the projectile had hit something
    internal bool HasHit()
    {
        return hit;
    }

    // Set Hit condition
    internal void SetHit(bool hit)
    {
        this.hit = hit;
    }

    // TODO: Projectile Hit
    // Do damage to a gameObject
    /*
    public void DoDamage(GameObject victim)
    {
        victim.TryGetComponent<HealthSystem>(out HealthSystem health);
        victim.TryGetComponent<KnockbackSystem>(out KnockbackSystem knockback);

        if (health)
        {
            // Damage health
            health.TakeDamage(attacker, damage);
        }

        if (knockback)
        {
            // Knockback push
            knockback.DoKnockback(knockbackForce, transform.up, !health.isDead, !health.isDead);
        }
    }*/
}
