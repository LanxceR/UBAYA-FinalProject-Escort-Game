using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game enemies manager script
/// </summary>
public class GameEnemyManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;

    // Enemy Prefabs
    [Header("All Enemy Prefabs")]
    [SerializeField] private EnemyScript[] enemyPrefabs; // List of ALL enemy prefabs possible to spawn

    #region Prefab Utilities
    // TODO: (DUPLICATE) Maybe put these methods in their corresponding scripts and load using Resources.Load
    /// <summary>
    /// Get an enemy instance of a type
    /// </summary>
    /// <param name="enemyType">The enemy type</param>
    /// <returns></returns>
    public EnemyScript GetEnemy(EnemyID enemyType)
    {
        // Find an enemy of a certain type
        foreach (EnemyScript e in enemyPrefabs)
        {
            if (e.id == enemyType) return e;
        }

        // If nothing is found, return null
        return null;
    }
    #endregion
}
