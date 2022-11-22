using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyID 
{
    ZOMBIE,
    Z_RUNNER,
    Z_BRUTE
}

/// <summary>
/// The main enemy script (or the hub)
/// </summary>
public class EnemyScript : MonoBehaviour, ICharacter
{
    // Enemy type
    [Header("Enemy ID")]
    [SerializeField]
    internal EnemyID id;

    // Enemy stats
    [Header("Enemy Stats")]
    [SerializeField]
    internal float health = 5f;
    [SerializeField]
    internal float baseSpeed = 0.5f;
    [SerializeField]
    internal bool knockbackImmune = false;

    // References of the enemy's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal EnemyAnimationScript enemyAnimationScript;
    [SerializeField]
    internal EnemyAIMovementScript enemyMovementScript;
    [SerializeField]
    internal EnemyAIAttackScript enemyAttackScript;
    [SerializeField]
    internal PathfindingScript pathfindingScript;
    // Generic sub-scripts
    [SerializeField]
    internal HealthScript healthScript;
    [SerializeField]
    internal ItemDropScript itemDropScript;
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

        // Add listener to Health's OnHealthReachedZero UnityEvent
        healthScript.OnHealthReachedZero.AddListener(EnemyDeath);
        healthScript.OnHealthReachedZero.AddListener(delegate { itemDropScript.SpawnItem(transform.position); });
    }

    void EnemyDeath()
    {
        if (enemyAttackScript)
        {
            enemyAttackScript.enabled = false;
        }
        if (pathfindingScript)
        {
            pathfindingScript.enabled = false;
        }
        if (recAggroScript)
        {
            recAggroScript.enabled = false;
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
