using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main projectile script (or the hub)
/// </summary>
public class ProjectileScript : MonoBehaviour
{
    // Projectile stats
    [Header("Projectile Stats")]
    [SerializeField] internal float damage = 99f;
    [SerializeField] internal float range = 10f;
    [SerializeField] internal float knockbackForce = 10f;
    [SerializeField] internal int pierceAmount = 0; // 0 means no piercing, 1 means pierces one enemy, etc
    [SerializeField] internal float pierceMultiplier = 0.5f; // Multiplier to apply to projectile after each pierced enemy
    [SerializeField] internal float minPierceMultiplier = 0.1f; // Min damage from pierce multiplier

    // Settings
    [Header("Projectile Settings")]
    [SerializeField] internal Vector3 spawnOffset = Vector3.zero;
    [SerializeField] internal string[] damageableTags;

    // References of the projectile's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal ProjectileHitScript projectileHitScript;
    [SerializeField]
    internal ProjectileMovementScript projectileMovementScript;
    [SerializeField]
    internal ProjectileAnimationScript projectileAnimationScript;
    [SerializeField]
    internal CollisionScript collisionScript;

    [Header("Misc Settings")]
    [SerializeField] private bool logDebug = false;

    // Methods to change stats
    // Set projectile range (lifetime based on dist. travelled)
    internal void SetRange(float range)
    {
        this.range = range;
    }
    internal void SetDamage(float damage)
    {
        this.damage = damage;
    }
    internal void SetKnockbackForce(float knockbackForce)
    {
        this.knockbackForce = knockbackForce;
    }
    internal void SetPierce(int pierceAmount, float pierceMultiplier, float minPierceMultiplier)
    {
        this.pierceAmount = pierceAmount;
        this.pierceMultiplier = pierceMultiplier;
        this.minPierceMultiplier = minPierceMultiplier;
    }
}
