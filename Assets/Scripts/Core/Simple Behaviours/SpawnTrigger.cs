using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviours for spawn trigger objects
/// </summary>
[RequireComponent(typeof(CollisionScript))]
public class SpawnTrigger : MonoBehaviour
{
    [Header("Main Setting")]
    [SerializeField] private CollisionScript collisionScript;
    [SerializeField] internal SpawnContext spawnType;
    [SerializeField] internal CompositeCollider2D spawnArea;

    // Variables
    [Header("Variables")]
    [SerializeField] private bool hasSpawned;

    // Components
    LevelManager lvlManager;

    // Start is called before the first frame update
    void Start()
    {
        lvlManager = Utilities.FindParentOfType<LevelManager>(transform, out _);

        if (collisionScript)
        {
            // Add listener to collision UnityEvents
            collisionScript.OnCollisionEnter?.AddListener(OnTriggerSpawn);
        }
    }

    private void OnTriggerSpawn()
    {
        if (hasSpawned)
            return;

        if (!lvlManager)
        {
            Debug.LogError("Level Manager not found!");
            return;
        }

        hasSpawned = lvlManager.SpawnEnemies(spawnType, spawnArea.bounds);
    }
}
