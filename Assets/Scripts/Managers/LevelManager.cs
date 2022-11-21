using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnContext
{
    IDLE,
    SMALL_WAVE,
    BIG_WAVE
}

/// <summary>
/// The level manager script
/// </summary>
public class LevelManager : MonoBehaviour
{
    #region Components
    //=============================================================//

    [Header("World Bounds")]
    [SerializeField] private CompositeCollider2D worldBound;

    //=============================================================//

    [Header("Triggers")]
    [SerializeField] private EdgeCollider2D spawnTriggerPrefab;
    [SerializeField] private EdgeCollider2D bigSpawnTriggerPrefab;
    [SerializeField] private EdgeCollider2D finishTriggerPrefab;

    [SerializeField] private GameObject triggersParent;

    //=============================================================//

    [Header("Blockade Prefabs")]
    [SerializeField] private GameObject blockadePrefab;

    [SerializeField] private GameObject blockadeParent;

    //=============================================================//

    [Header("Enemies")]
    [SerializeField] private GameObject enemiesParent;

    //=============================================================//

    [Header("Map Setting")]
    [SerializeField] private GameObject map;
    [SerializeField] private Bounds mapBounds;
    #endregion

    #region Variables
    [Header("Triggers In Scene")]
    [SerializeField] private List<EdgeCollider2D> spawnTriggers;
    [SerializeField] private List<EdgeCollider2D> bigSpawnTriggers;
    [SerializeField] private EdgeCollider2D finishTrigger;

    [Header("In-Game Values")]
    [SerializeField] internal float timePassed;
    internal Coroutine timerCoroutine;
    [SerializeField] internal int totalEnemyCount;
    [SerializeField] internal int enemyCountLeft;
    [SerializeField] internal List<Spawnable> enemiesToSpawn;

    [Header("In-Game Instances")]
    [SerializeField] internal List<EnemyScript> activeEnemies;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Assign Main Camera CinemachineConfiner2D's bounding shape
        GameManager.Instance.InGameCameras.mainCameraConfiner2D.m_BoundingShape2D = worldBound;

        // Initialization
        SetupLevel();

        totalEnemyCount = GameManager.Instance.LoadedMissionData.zombieCount;
        enemyCountLeft = totalEnemyCount;

        enemiesToSpawn = GameManager.Instance.LoadedMissionData.enemies;

        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }
    }

    private void SetupLevel()
    {
        // For now, roughly divide the amount of idle, small wave spawns, and big wave spawns by these proportions
        // Idle -> 10% of totalEnemy
        // Small Wave (Total) -> 40% of totalEnemy
        // Big Wave (Total) -> 50% of totalEnemy

        Vector2 pos;
        float padding = 5f; // Padding area for triggers to always spawn within the padded area (from left OR right)

        // Randomize spawn trigger amounts
        int spawnTriggerAmount = Random.Range(5, 11);
        pos = map.transform.position;
        for (int i = 0; i < spawnTriggerAmount; i++)
        {
            // Randomize spawn trigger position
            // To roughly distribute evenly, randomize each spawn trigger each in their own area,
            // E.g the 2nd out of 10 triggers position would be randomized between 1/10 to 2/10, so on & so forth
            pos.x = Random.Range(
                (mapBounds.extents.x - (padding * 2)) * i / spawnTriggerAmount,
                (mapBounds.extents.x - (padding * 2)) * (i + 1) / spawnTriggerAmount
                ) + padding;
            spawnTriggers.Add(Instantiate(spawnTriggerPrefab, pos, Quaternion.identity, triggersParent.transform));
        }

        // Randomize big spawn trigger amounts
        int bigSpawnTriggerAmount = Random.Range(2, 5);
        pos = map.transform.position;
        for (int i = 0; i < bigSpawnTriggerAmount; i++)
        {
            // Randomize spawn trigger position
            // To roughly distribute evenly, randomize each spawn trigger each in their own area,
            // E.g the 2nd out of 10 triggers position would be randomized between 1/10 to 2/10, so on & so forth
            pos.x = Random.Range(
                (mapBounds.extents.x - (padding * 2)) * i / bigSpawnTriggerAmount,
                (mapBounds.extents.x - (padding * 2)) * (i + 1) / bigSpawnTriggerAmount
                ) + padding;
            bigSpawnTriggers.Add(Instantiate(bigSpawnTriggerPrefab, pos, Quaternion.identity, triggersParent.transform));
        }

        // Put finish line at the end (half the padding from the right)
        pos = map.transform.position;
        pos.x = mapBounds.extents.x - (padding / 2);
        finishTrigger = Instantiate(finishTriggerPrefab, pos, Quaternion.identity, triggersParent.transform);
    }

    private IEnumerator TimerCoroutine()
    {
        // Loop forever until stopped
        while (true)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
    }

    private bool SpawnEnemies(SpawnContext context, Vector2 spawnPosition)
    {
        int amountToSpawn = 0;
        bool hasSpawnedSomething = false;

        switch (context)
        {
            case SpawnContext.IDLE:
                amountToSpawn = Mathf.RoundToInt(totalEnemyCount / 0.05f);
                break;
            case SpawnContext.SMALL_WAVE:
                amountToSpawn = Mathf.RoundToInt(totalEnemyCount / (0.45f / spawnTriggers.Count));
                break;
            case SpawnContext.BIG_WAVE:
                amountToSpawn = Mathf.RoundToInt(totalEnemyCount / (0.5f / bigSpawnTriggers.Count));
                break;
        }

        // Add total weight
        int totalWeight = 0;
        foreach (Spawnable s in enemiesToSpawn)
        {
            totalWeight += s.spawnWeight;
        }

        // Spawn enemies
        for (int i = 0; i < amountToSpawn; i++)
        {
            // Randomize enemy to spawn
            EnemyScript enemyToSpawn = null;

            // Generate a random number from 1 - totalWeight
            int rand = Random.Range(1, totalWeight + 1);

            int pos = 0;
            for (int j = 0; j < enemiesToSpawn.Count; j++)
            {
                if (rand <= enemiesToSpawn[j].spawnWeight + pos)
                {
                    enemyToSpawn = enemiesToSpawn[j].prefab.GetComponent<EnemyScript>();
                    break;
                }
                pos += enemiesToSpawn[j].spawnWeight;
            }

            if (enemyToSpawn)
            {
                // TODO: Randomize spawn position, also try and spawn from outside the map, OR work on spawn position any other way
                // If there's an enemy to spawn, then instantiate that enemy
                activeEnemies.Add(Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, enemiesParent.transform));
                hasSpawnedSomething = true;
            }
        }

        return hasSpawnedSomething;
    }
}
