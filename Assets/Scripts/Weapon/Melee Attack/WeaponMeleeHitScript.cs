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
    internal List<GameObject> victims = new List<GameObject>(); // Victims list to prevent a melee attack from repeatedly registering consecutive hits
    
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

    internal void OnHit(GameObject victim)
    {
        // If victim has already been hit by this weapon attack, then return
        if (victims.Contains(victim)) return;

        if (Utilities.FindParent<HealthScript>(victim.transform, out Transform parent))
            Debug.Log($"{attacker.name}'s attack has hit {parent?.name}!");

        // Try to damage victim
        Hit(victim);

        // Add victim to victim's list
        victims.Add(victim);
    }

    // Hit an gameObject (and do various hitting related behaviours)
    private void Hit(GameObject victim)
    {
        // For abbreviation
        var w = weaponScript.weaponAttackScript as WeaponMeleeAttackScript;


        // Fetch victim's health on their parent gameobject
        HealthScript health = Utilities.FindParentOfType<HealthScript>(victim.transform, out _);

        if (health)
        {
            // Damage health
            health.TakeDamage(attacker, w.damage);
        }


        // Fetch victim's knockback on their parent gameobject
        KnockbackScript knockback = Utilities.FindParentOfType<KnockbackScript>(victim.transform, out _);

        // Fetch victim's collider
        victim.TryGetComponent(out Collider2D collider);

        if (knockback)
        {
            // Get knockback direction
            Vector2 dir = collider.bounds.center - transform.position;

            // Knockback push
            knockback.DoKnockback(w.knockbackForce, dir.normalized, !health.IsDead, !health.IsDead);
        }


        // Fetch victim's knockback on their parent gameobject
        ReceiveAggroScript aggro = Utilities.FindParentOfType<ReceiveAggroScript>(victim.transform, out _);

        if (aggro)
        {
            // Force aggro
            aggro.ForceAggroTarget(attacker.transform, 15f);
        }
    }
}
