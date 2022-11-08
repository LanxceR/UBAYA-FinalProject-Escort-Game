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
    internal float baseSpeed = 0.5f;
    [SerializeField]
    internal bool knockbackImmune = false;

    // TODO: Implement other subscripts for the enemy
    // TODO: Implement melee attack for enemy zombies
    // References of the enemy's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal EnemyAnimationScript enemyAnimationScript;
    [SerializeField]
    internal EnemyAIMovementScript enemyMovementScript;
    [SerializeField]
    internal PathfindingScript pathfindingScript;
    [SerializeField]
    internal HealthScript healthScript;
    [SerializeField]
    internal KnockbackScript knockbackScript;
    [SerializeField]
    internal ReceiveAggroScript recAggroScript;

    // Start is called before the first frame update
    void Start()
    {
        // Set health
        healthScript.MaxHealth = health;

        // Set knockback immunity
        knockbackScript.knockbackImmune = this.knockbackImmune;
    }
}
