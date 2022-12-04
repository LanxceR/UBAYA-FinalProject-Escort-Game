using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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

        // Update A* graphs after a blockade is destroyed
        UpdateGraphs(collider.bounds);
    }

    // Use to update A* graph on this part of bounds
    public void UpdateGraphs(Bounds bounds)
    {
        var guo = new GraphUpdateObject(bounds);

        // Set some settings
        guo.updatePhysics = true;

        // Update graphs
        AstarPath.active.UpdateGraphs(guo);
    }
}
