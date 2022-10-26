using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The weapon melee hit script (handles all weapon hitting behaviour (if the weapon is able to))
/// </summary>
[RequireComponent(typeof(WeaponScript), typeof(WeaponMeleeAttackScript), typeof(CollisionScript))]
[RequireComponent(typeof(Rigidbody2D))]
public class WeaponMeleeHitScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private WeaponScript weaponScript;

    [SerializeField]
    internal CollisionScript collisionScript;

    // Variables
    private GameObject attacker;
    private bool hit; // To prevent bullet from repeatedly registering consecutive hits

    // Start is called before the first frame update
    void Start()
    {
        if (collisionScript)
        {
            // Add listener to collision UnityEvents
            collisionScript.OnCollisionEnterGO?.AddListener(OnHit);
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

    internal void OnHit(GameObject victim)
    {
        // TODO: Perform checking and/or retrieve the correct parent victim gameobject
        Debug.Log($"{attacker.name}'s attack has hit {victim.transform.parent.parent.name}!");

        // Try to damage victim
        Hit(victim);
    }

    // Hit an gameObject (and do various hitting related behaviours)
    private void Hit(GameObject victim)
    {
        // For abbreviation
        var w = weaponScript.weaponAttackScript as WeaponMeleeAttackScript;

        // TODO: Find parent instead of climbing up manually
        // Fetch victim's health on their parent gameobject
        victim.transform.parent.parent.TryGetComponent(out HealthScript health);

        if (health)
        {
            // Damage health
            health.TakeDamage(attacker, w.damage);
        }

        // Fetch victim's knockback script on their parent gameobject
        victim.transform.parent.parent.TryGetComponent(out KnockbackScript knockback);
        // Fetch victim's collider
        victim.TryGetComponent(out Collider2D collider);

        if (knockback)
        {
            // Get knockback direction
            Vector2 dir = collider.bounds.center - transform.position;

            // Knockback push
            knockback.DoKnockback(w.knockbackForce, dir.normalized, !health.IsDead, !health.IsDead);
        }
    }
}