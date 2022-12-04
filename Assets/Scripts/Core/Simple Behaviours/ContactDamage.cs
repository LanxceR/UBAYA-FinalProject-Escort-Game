using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object deals damage on contact
/// </summary>
[RequireComponent(typeof(CollisionScript))]
public class ContactDamage : MonoBehaviour
{
    // Components
    private CollisionScript collisionScript;

    // Variables
    [SerializeField]
    private float damage = 1f;
    [SerializeField]
    private float force = 10f;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        collisionScript = GetComponent<CollisionScript>();
    }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        if (collisionScript)
        {
            // Add listener to collision UnityEvents
            collisionScript.OnCollisionEnterGO?.AddListener(OnHit);
        }
    }

    internal void OnHit(GameObject victim)
    {
        if (Utilities.FindParent<HealthScript>(victim.transform, out Transform parent))
            Debug.Log($"{gameObject.name} has hit {parent.name}!");

        // Try to damage victim
        Hit(victim);
    }

    // Hit an gameObject (and do various hitting related behaviours)
    private void Hit(GameObject victim)
    {
        // Fetch victim's health on their parent gameobject
        HealthScript health = Utilities.FindParentOfType<HealthScript>(victim.transform, out _);

        if (health)
        {
            // Damage health
            health.TakeDamage(gameObject, damage);
        }

        // Fetch victim's knockback on their parent gameobject
        KnockbackScript knockback = Utilities.FindParentOfType<KnockbackScript>(victim.transform, out _);

        if (knockback)
        {
            // Knockback push
            knockback.DoKnockback(force, Utilities.VectorBetweenTwoPoints(victim.transform.position, gameObject.transform.position).normalized, !health.IsDead, !health.IsDead);
        }
    }
}
