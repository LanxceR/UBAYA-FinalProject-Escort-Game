using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
    [Header("===INITIALIZATION===")]
    //=============================================================//

    [Header("Player Spawn Setting")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform escorteeSpawnPoint;

    //=============================================================//

    [Header("World Bounds")]
    [SerializeField] private CompositeCollider2D worldBound;

    //=============================================================//

    [Header("Triggers")]
    [SerializeField] private SpawnTrigger spawnTriggerPrefab;
    [SerializeField] private SpawnTrigger bigSpawnTriggerPrefab;
    [SerializeField] private FinishTrigger finishTriggerPrefab;

    [SerializeField] private GameObject triggersParent;

    //=============================================================//

    [Header("Blockades")]
    [SerializeField] private BlockadeScript[] blockadePrefabs;
    [SerializeField] private GameObject blockadeParent;

    //=============================================================//

    [Header("Enemies")]
    [SerializeField] private GameObject enemiesParent;

    //=============================================================//

    [Header("Map Setting")]
    [SerializeField] private GameObject map;
    [SerializeField] private Bounds mapBounds;
    
    //=============================================================//

    [Header("Idle Spawn Area Setting")]
    [SerializeField] private CompositeCollider2D idleSpawnArea;
    #endregion

    #region Variables
    [Header("===IN-GAME===")]

    [Header("In-Game Instances")]
    [SerializeField] private List<SpawnTrigger> spawnTriggers;
    [SerializeField] private List<SpawnTrigger> bigSpawnTriggers;
    [SerializeField] private FinishTrigger finishTrigger;
    [SerializeField] internal List<EnemyScript> enemiesInScene;
    [SerializeField] internal List<BlockadeScript> blockadesInScene;

    [Header("In-Game Values")]
    [SerializeField] internal float timePassed;
    internal Coroutine timerCoroutine;
    [SerializeField] internal int totalEnemyCount;
    [SerializeField] internal int enemyCountLeft;
    [SerializeField] internal List<Spawnable> enemiesToSpawn;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Assign Main Camera CinemachineConfiner2D's bounding shape
        GameManager.Instance.InGameCameras.mainCameraConfiner2D.m_BoundingShape2D = worldBound;

        // Initialization
        mapBounds = worldBound.bounds;
        SetupLevel();

        // Spawn players & escortees
        GameManager.Instance.gamePlayer.TrySpawnPlayer(playerSpawnPoint);
        GameManager.Instance.gameEscortee.TrySpawnEscortee(escorteeSpawnPoint);

        // Enemies
        totalEnemyCount = GameManager.Instance.LoadedMissionData.zombieCount;
        enemyCountLeft = totalEnemyCount;

        enemiesToSpawn = GameManager.Instance.LoadedMissionData.enemies;

        SpawnEnemies(SpawnContext.IDLE, idleSpawnArea.bounds);

        // Timer
        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }
    }

    #region Level Setups
    private void SetupLevel()
    {
        SetupTriggers();

        SetupBlockades();
    }

    private void SetupTriggers()
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
                (mapBounds.extents.x * 2 - (padding * 2)) * i / spawnTriggerAmount,
                (mapBounds.extents.x * 2 - (padding * 2)) * (i + 1) / spawnTriggerAmount
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
                (mapBounds.extents.x * 2 - (padding * 2)) * i / bigSpawnTriggerAmount,
                (mapBounds.extents.x * 2 - (padding * 2)) * (i + 1) / bigSpawnTriggerAmount
                ) + padding;
            bigSpawnTriggers.Add(Instantiate(bigSpawnTriggerPrefab, pos, Quaternion.identity, triggersParent.transform));
        }

        // Put finish line at the end (half the padding from the right)
        pos = map.transform.position;
        pos.x = mapBounds.extents.x * 2 - (padding / 2);
        finishTrigger = Instantiate(finishTriggerPrefab, pos, Quaternion.identity, triggersParent.transform);
    }

    private void SetupBlockades()
    {
        Vector2 pos;
        float padding = 5f; // Padding area for triggers to always spawn within the padded area (from left OR right)

        // Randomize spawn trigger amounts
        int blockadesAmount = Random.Range(1, 4);
        pos = map.transform.position;
        for (int i = 0; i < blockadesAmount; i++)
        {
            // Randomize blockades position
            // To roughly distribute evenly, randomize each blockades in their own area,
            // E.g the 2nd out of 10 triggers position would be randomized between 1/10 to 2/10, so on & so forth
            pos.x = Random.Range(
                (mapBounds.extents.x * 2 - (padding * 2)) * i / blockadesAmount,
                (mapBounds.extents.x * 2 - (padding * 2)) * (i + 1) / blockadesAmount
                ) + padding;
            BlockadeScript blockade = Instantiate(blockadePrefabs[Random.Range(0, blockadePrefabs.Length)], pos, Quaternion.identity, blockadeParent.transform);
            blockadesInScene.Add(blockade);
            UpdateGraphs(blockade.collider.bounds);
        }
    } 
    #endregion

    #region Enemy Spawning
    private Vector2 GetRandomPosition(Bounds area)
    {
        float xPos = Random.Range(area.min.x, area.max.x);
        float yPos = Random.Range(area.min.y, area.max.y);

        return new Vector2(xPos, yPos);
    }

    internal bool SpawnEnemies(SpawnContext context, Bounds spawnArea)
    {
        int amountToSpawn = 0;
        bool hasSpawnedSomething = false;
        Vector2 spawnPos = Vector2.zero;

        // Determine amount to spawn
        switch (context)
        {
            case SpawnContext.IDLE:
                amountToSpawn = Mathf.Clamp(Mathf.RoundToInt(totalEnemyCount * 0.1f), 0, enemyCountLeft);
                break;
            case SpawnContext.SMALL_WAVE:
                amountToSpawn = Mathf.Clamp(Mathf.RoundToInt(totalEnemyCount * (0.4f / spawnTriggers.Count)), 0, enemyCountLeft);
                break;
            case SpawnContext.BIG_WAVE:
                amountToSpawn = Mathf.Clamp(Mathf.RoundToInt(totalEnemyCount * (0.5f / bigSpawnTriggers.Count)), 0, enemyCountLeft);
                break;
        }

        // Add total weight
        float totalWeight = 0;
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
            float rand = Random.Range(1, totalWeight);

            // Weighted randomize pick an enemy to spawn
            float pos = 0;
            for (int j = 0; j < enemiesToSpawn.Count; j++)
            {
                if (rand <= enemiesToSpawn[j].spawnWeight + pos)
                {
                    enemyToSpawn = enemiesToSpawn[j].prefab.GetComponent<EnemyScript>();
                    break;
                }
                pos += enemiesToSpawn[j].spawnWeight;
            }

            // Get random position inside spawn area
            spawnPos = GetRandomPosition(spawnArea);

            if (enemyToSpawn)
            {
                // If there's an enemy to spawn, then instantiate that enemy
                EnemyScript spawnedEnemy = Instantiate(enemyToSpawn, spawnPos, Quaternion.identity, enemiesParent.transform);
                enemiesInScene.Add(spawnedEnemy);

                if (context != SpawnContext.IDLE)
                {
                    // Set enemy to walk left after spawning
                    spawnedEnemy.enemyMovementScript.SetDirection(Vector2.left);
                }

                // Subtract enemy count left
                enemyCountLeft--;
                // Set flag
                hasSpawnedSomething = true;
            }
        }

        Debug.Log($"Spawned {amountToSpawn} enemies");
        return hasSpawnedSomething;
    }
    #endregion

    internal void TryEndLevel(bool reachedDestination)
    {
        bool allWinConditionsMet = true;

        if (reachedDestination)
        {
            foreach (EnemyScript e in enemiesInScene)
            {
                if (e.healthScript)
                {
                    if (!e.healthScript.IsDead)
                    {
                        // Not all enemies are dead
                        Debug.Log($"Not all enemies are dead!");
                        allWinConditionsMet = false;
                        break;
                    }
                }
            }
        }
        else
        {
            // Not everyone has reached the destination
            Debug.Log($"Not everyone has reached the destination yet!");
            allWinConditionsMet = false;
        }

        if (allWinConditionsMet)
        {
            // Mission successful
            GameManager.Instance.gameState.GameOver(GameOverEvent.MISSION_SUCCESS);
        }
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
