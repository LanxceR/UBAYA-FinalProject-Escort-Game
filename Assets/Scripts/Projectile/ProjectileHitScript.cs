using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// The projectile damage script (handles all projectile damaging action)
/// </summary>
public class ProjectileHitScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private ProjectileScript projectileScript;

    // Variables
    [SerializeField]
    private int hits; // Total hits
    private GameObject attacker;
    private List<GameObject> victims = new List<GameObject>(); // Victims list to prevent a projectile from repeatedly registering consecutive hits

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        if (projectileScript.collisionScript)
        {
            // Add listener to collision UnityEvents
            projectileScript.collisionScript.OnCollisionEnterGO?.AddListener(OnHit);
        }
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Clear victims list
        if (victims.Any())
            victims.Clear();

        // Set hit variable to 0 (bullet hasn't hit anything yet)
        hits = 0;

        // Enable collider
        projectileScript.collisionScript.EnableCollider();
    }

    // Update is called once per frame
    void Update()
    {
        if (HasHit())
        {
            // If projectile has hit something,

            // Disable collider
            projectileScript.collisionScript.DisableCollider();

            // Stop moving
            projectileScript.projectileMovementScript.StopMoving();
            
            // TODO: Projectile hit and destroy animation
            gameObject.SetActive(false);
        }
    }

    internal void CheckForHitsInTheWay(float velocity, Vector2 direction)
    {
        // Check if there's a hit victim in this projectile's travel path
        // This is to prevent projectile from phasing through victims due too high velocity
        // NOTE: Subtract position with spawnOffset (this is due to projectile's position is offset from the model when spawned)
        // Use layerMask ActorHitbox ( 1 << 7 )
        RaycastHit2D victim = Physics2D.Raycast(transform.position - projectileScript.spawnOffset, direction, velocity, 1 << 7);

        if (victim && projectileScript.collisionScript.CheckTargetedTags(victim.collider.gameObject) != null)
        {
            // Something is in the way, try and hit that something
            OnHit(victim.collider.gameObject);
        }
    }

    internal void SetAttacker(GameObject attacker)
    {
        this.attacker = attacker;
    }

    // Check if the projectile had hit as much as it is capable of (pierce amount)
    internal bool HasHit()
    {
        return hits > projectileScript.pierceAmount;
    }

    internal void OnHit(GameObject victim)
    {
        // If victim has already been hit by this projectile, then return
        if (victims.Contains(victim)) return;

        // If bullet has hit something, don't hit any more victims
        if (HasHit()) return;

        if (Utilities.FindParent<HealthScript>(victim.transform, out Transform parent))
            Debug.Log($"{attacker.name}'s attack has hit {parent?.name}!");

        foreach (string tag in projectileScript.damageableTags)
        {
            if (victim.gameObject.CompareTag(tag))
            {
                // Try to damage victim
                Hit(victim);
            }
        }

        var p = projectileScript; // Abbreviation
        // Multiply damage by pierce multiplier, clamp to minimum pierce multiplier
        // Floored to the largest integer
        p.damage = (p.damage * p.pierceMultiplier) > (p.damage * p.minPierceMultiplier) ? 
                    Mathf.FloorToInt(p.damage * p.pierceMultiplier) : 
                    Mathf.FloorToInt(p.damage * p.minPierceMultiplier);

        // Add victim to victim's list
        victims.Add(victim);

        // Increment bullet hit by 1; bullet has hit something
        hits++;
    }

    // Hit an gameObject (and do various hitting related behaviours)
    private void Hit(GameObject victim)
    {
        // Fetch victim's health on their parent gameobject
        HealthScript health = Utilities.FindParentOfType<HealthScript>(victim.transform, out _);

        if (health)
        {
            // Damage health
            health.TakeDamage(attacker, projectileScript.damage);
        }


        // Fetch victim's knockback on their parent gameobject
        KnockbackScript knockback = Utilities.FindParentOfType<KnockbackScript>(victim.transform, out _);

        if (knockback)
        {
            // Knockback push
            knockback.DoKnockback(projectileScript.knockbackForce, projectileScript.projectileMovementScript.GetDirection(), !health.IsDead, !health.IsDead);
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
