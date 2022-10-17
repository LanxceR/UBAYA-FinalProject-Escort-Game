using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The projectile damage script (handles all projectile damaging action)
/// </summary>
public class ProjectileHitScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private ProjectileScript projectileScript;

    // Variables
    private GameObject attacker;
    private bool hit; // To prevent bullet from repeatedly registering consecutive hits

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Set hit variable to false (bullet hasn't hit anything yet)
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Projectile knockback
        if (HasHit())
        {
            // If projectile has hit something,

            // Disable collider
            projectileScript.projectileCollisionScript.DisableCollider();

            // Stop moving
            projectileScript.projectileMovementScript.StopMoving();
            
            // TODO: Projectile hit and destroy animation
            gameObject.SetActive(false);
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
    private void SetHit(bool hit)
    {
        this.hit = hit;
    }

    internal void OnHit(GameObject victim)
    {
        Debug.Log($"{attacker.name}'s projectile has hit {victim.transform.parent.parent.name}!");

        // Set bullet hit to true; bullet has hit something
        SetHit(true);
    }

    // Do damage to a gameObject
    /*
    private void DoDamage(GameObject victim)
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
