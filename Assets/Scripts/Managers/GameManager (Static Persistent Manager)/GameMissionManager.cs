using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game mission manager script
/// </summary>
public enum MissionDifficulty { EASY, INTERMEDIATE, HARD, FINAL}
public class GameMissionManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;

    // Enemy Prefabs
    [Header("All Enemy Prefabs")]
    [SerializeField] private EnemyScript[] enemyPrefabs; // List of ALL enemy prefabs possible to spawn

    // Enemy Prefabs
    [Header("All Weapon Prefabs")]
    [SerializeField] private WeaponScript[] weaponPrefabs; // List of ALL enemy prefabs possible to spawn

    private EnemyScript GetEnemy(EnemyID enemyType)
    {
        // Find an enemy of a certain type
        foreach (EnemyScript e in enemyPrefabs)
        {
            if (e.id == enemyType) return e;
        }

        // If nothing is found, return null
        return null;
    }

    // Generate missions for that day
    public void GenerateMissions(int day)
    {
        if (gameManager.LoadedGameData.difficulty == Difficulty.HARDCORE)
        {
            // HARDCORE Mission Gen
            if (day <= 3)
            {
                // Day 1-3
                CreateMission(MissionDifficulty.EASY, HazardRating.NORMAL, 0);
                CreateMission(MissionDifficulty.EASY, HazardRating.INFESTED, 1);
                CreateMission(MissionDifficulty.EASY, HazardRating.OVERRUN, 2);
            }
            else if (day <= 6)
            {
                // Day 4-6
                CreateMission(MissionDifficulty.INTERMEDIATE, HazardRating.NORMAL, 0);
                CreateMission(MissionDifficulty.INTERMEDIATE, HazardRating.INFESTED, 1);
                CreateMission(MissionDifficulty.INTERMEDIATE, HazardRating.OVERRUN, 2);
            }
            else if (day <= 9)
            {
                // Day 7-9
                CreateMission(MissionDifficulty.HARD, HazardRating.NORMAL, 0);
                CreateMission(MissionDifficulty.HARD, HazardRating.INFESTED, 1);
                CreateMission(MissionDifficulty.HARD, HazardRating.OVERRUN, 2);
            }
            else if (day == 10)
            {
                // Day 10 (Final Day)
                CreateMission(MissionDifficulty.FINAL, HazardRating.APOCALYPSE, 0);
            }
            else
            {
                // Post-game
                CreateMission(MissionDifficulty.HARD, HazardRating.APOCALYPSE, 0);
                CreateMission(MissionDifficulty.HARD, HazardRating.APOCALYPSE, 1);
                CreateMission(MissionDifficulty.FINAL, HazardRating.APOCALYPSE, 2);
            }
        }
        else
        {
            // CASUAL Mission Gen

            if (day <= 3)
            {
                // Day 1-3
                CreateMission(MissionDifficulty.EASY, HazardRating.NORMAL, 0);
                CreateMission(MissionDifficulty.EASY, HazardRating.INFESTED, 1);
                CreateMission(MissionDifficulty.EASY, HazardRating.OVERRUN, 2);
            }
            else if (day <= 6)
            {
                // Day 4-6
                CreateMission(MissionDifficulty.INTERMEDIATE, HazardRating.NORMAL, 0);
                CreateMission(MissionDifficulty.INTERMEDIATE, HazardRating.INFESTED, 1);
                CreateMission(MissionDifficulty.INTERMEDIATE, HazardRating.OVERRUN, 2);
            }
            else if (day > 6)
            {
                // Day 7 onwards
                CreateMission(MissionDifficulty.HARD, HazardRating.NORMAL, 0);
                CreateMission(MissionDifficulty.HARD, HazardRating.INFESTED, 1);
                CreateMission(MissionDifficulty.HARD, HazardRating.OVERRUN, 2);
            }

            // Generate final mission if the condition is met
            if (gameManager.LoadedGameData.missionsCompleted >= 5)
                CreateMission(MissionDifficulty.FINAL, HazardRating.APOCALYPSE, 2);
        }
    }

    /// <summary>
    /// Create/generate a mission and store it in mission index
    /// <para><b>Note:</b> Use difficulty "FINAL" and hazard "APOCALYPSE" for final missions.</para>
    /// </summary>
    /// <param name="difficulty">The "difficulty" of the mission, determining which type of enemies can appear. Use this to introduce different enemy types at a certain stage of the game.</param>
    /// <param name="hazard">The hazard rating of the mission, determining the total amount of zombie to be spawned in a mission. Use this for mission variety in a single day.</param>
    /// <param name="index">The index to store this mission in. Generally proportional with HazardRating, e.g 0 = NORMAL, 1 = INFESTED, 2 = OVERRUN</param>
    internal void CreateMission(MissionDifficulty difficulty, HazardRating hazard, int index)
    {
        // Randomize if escortee have weapons or not
        bool escorteeHasWeapon = Random.Range(0, 2) == 1 ? true : false;

        // Randomize Zombie Count & base reward
        int zombieCount;
        float baseReward = 1000;
        switch (hazard)
        {
            case HazardRating.NORMAL:
                zombieCount = Random.Range(25, 51);
                baseReward *= 1f;
                break;
            case HazardRating.INFESTED:
                zombieCount = Random.Range(50, 101);
                baseReward *= 1.5f;
                break;
            case HazardRating.OVERRUN:
                zombieCount = Random.Range(100, 151);
                baseReward *= 2f;
                break;
            case HazardRating.APOCALYPSE:
                zombieCount = Random.Range(200, 301);
                baseReward *= 3f;
                break;
            default:
                zombieCount = Random.Range(50, 101);
                baseReward *= 1f;
                break;
        }

        // Randomize Enemy Types & escortee type
        List<Spawnable> enemies = new List<Spawnable>();
        bool randomizeEscortee = false;
        switch (difficulty)
        {
            case MissionDifficulty.EASY:
                enemies.Add(new Spawnable(GetEnemy(EnemyID.ZOMBIE).gameObject, 100));
                // Randomize escortee selection
                randomizeEscortee = Random.Range(0, 2) == 1 ? true : false;
                break;
            case MissionDifficulty.INTERMEDIATE:
                enemies.Add(new Spawnable(GetEnemy(EnemyID.ZOMBIE).gameObject, 80));
                enemies.Add(new Spawnable(GetEnemy(EnemyID.Z_BRUTE).gameObject, 20));
                // Randomize escortee selection
                randomizeEscortee = Random.Range(0, 2) == 1 ? true : false;
                break;
            case MissionDifficulty.HARD:
                enemies.Add(new Spawnable(GetEnemy(EnemyID.ZOMBIE).gameObject, 60));
                enemies.Add(new Spawnable(GetEnemy(EnemyID.Z_BRUTE).gameObject, 20));
                enemies.Add(new Spawnable(GetEnemy(EnemyID.Z_RUNNER).gameObject, 20));
                // Randomize escortee selection
                randomizeEscortee = Random.Range(0, 2) == 1 ? true : false;
                break;
            case MissionDifficulty.FINAL:
                enemies.Add(new Spawnable(GetEnemy(EnemyID.ZOMBIE).gameObject, 40));
                enemies.Add(new Spawnable(GetEnemy(EnemyID.Z_BRUTE).gameObject, 30));
                enemies.Add(new Spawnable(GetEnemy(EnemyID.Z_RUNNER).gameObject, 30));
                // NO RANDOM ESCORTEE FOR FINAL MISSION
                break;
            default:
                enemies.Add(new Spawnable(GetEnemy(EnemyID.ZOMBIE).gameObject, 60));
                enemies.Add(new Spawnable(GetEnemy(EnemyID.Z_BRUTE).gameObject, 20));
                enemies.Add(new Spawnable(GetEnemy(EnemyID.Z_RUNNER).gameObject, 20));
                // Randomize escortee selection
                randomizeEscortee = Random.Range(0, 2) == 1 ? true : false;
                break;
        }

        EscorteeScript vehicle;
        if (randomizeEscortee)
        {
            // Choose random vehicle
            vehicle = gameManager.gameEscortee.EscorteePrefabs[Random.Range(0, gameManager.gameEscortee.EscorteePrefabs.Length)];

            // Store mission to it's index
            gameManager.MissionDatas[index] = new MissionData(vehicle, escorteeHasWeapon, zombieCount, baseReward, enemies);
        }
        else
        {
            // Store mission to it's index
            gameManager.MissionDatas[index] = new MissionData(escorteeHasWeapon, zombieCount, baseReward, enemies);
        }
    }
}
