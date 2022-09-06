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
    [SerializeField] internal float range = 5f;
    [SerializeField] internal float damage = 1f;
    [SerializeField] internal float knockbackForce = 10f;

    // References of the projectile's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal ProjectileHitScript projectileHitScript;
    // References of the projectile's sub-scripts
    [SerializeField]
    internal ProjectileMovementScript projectileMovementScript;

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
}
