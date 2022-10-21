using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main enemy script (or the hub)
/// </summary>
public class EnemyScript : MonoBehaviour, ICharacter
{
    // Enemy stats
    [Header("Enemy Stats")]
    [SerializeField]
    internal float health = 5f;
    [SerializeField]
    internal float speed = 0.5f;
    [SerializeField]
    internal bool knockbackImmune = false;

    // TODO: Implement other subscripts for the enemy
    // References of the enemy's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal EnemyAnimationScript enemyAnimationScript;
    [SerializeField]
    internal HealthScript healthScript;
    [SerializeField]
    internal KnockbackScript knockbackScript;

    // Start is called before the first frame update
    void Start()
    {
        // Set health
        healthScript.MaxHealth = health;

        // Set knockback immunity
        knockbackScript.knockbackImmune = this.knockbackImmune;
    }
}
