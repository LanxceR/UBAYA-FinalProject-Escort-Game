using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main blockade script
/// </summary>
public class BlockadeScript : MonoBehaviour
{
    // Escortee stats
    [Header("Blockade Stats")]
    [SerializeField]
    internal float health = 50f;

    [Header("Collider Settings")]
    [SerializeField]
    internal Collider2D collider;

    // References of the escortee's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal HealthScript healthScript;

    // Start is called before the first frame update
    void Start()
    {
        // Set health
        healthScript.MaxHealth = health;

        // Add listener to Health's OnHealthReachedZero UnityEvent
        healthScript.OnHealthReachedZero.AddListener(BlockadeDestroyed);
    }

    void BlockadeDestroyed()
    {
        // Disable this blockade after it's destroyed
        gameObject.SetActive(false);
    }
}
