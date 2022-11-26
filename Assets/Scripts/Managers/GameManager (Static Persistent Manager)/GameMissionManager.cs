using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The game mission manager script
/// </summary>
public enum MissionDifficulty { EASY, INTERMEDIATE, HARD, FINAL}
public enum MissionEndEvent
{
    MISSION_SUCCESS,
    MISSION_FAILED,
    TUTORIAL_COMPLETE
}
public class GameMissionManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    public GameManager gameManager;

    // Escort Scenes
    [Header("Possible Escort Scenes")]
    [SerializeField] internal SceneName[] escortScenes; // List of all valid escort scenes

    // ========================================================================================== //

    // Unity Events
    // Subbed at: InGameGameOverUIScript
    /// <summary>
    /// Event invoked when there's a game over
    /// </summary>
    internal UnityAction<MissionEndEvent> OnMissionEnd;

    public void MissionEnd(MissionEndEvent missionEndEvent)
    {
        switch (missionEndEvent)
        {
            case MissionEndEvent.MISSION_SUCCESS:
                Debug.Log($"Mission Successful!");
                gameManager.LoadedGameData.missionsCompleted++;
                gameManager.LoadedGameData.daysPassed++;

                // Invoke OnMissionEnd event
                OnMissionEnd?.Invoke(MissionEndEvent.MISSION_SUCCESS);

                if (gameManager.LoadedMissionData.isFinalMission)
                {
                    // Call GameOver Ending
                    gameManager.gameState.GameOver(GameOverEvent.ENDING);
                }
                break;
            case MissionEndEvent.MISSION_FAILED:
                Debug.Log($"Mission Failed!");
                gameManager.LoadedGameData.missionsFailed++;
                gameManager.LoadedGameData.daysPassed++;

                // Invoke OnMissionEnd event
                OnMissionEnd?.Invoke(MissionEndEvent.MISSION_FAILED);

                // If Hardcore difficulty
                if (gameManager.LoadedGameData.difficulty == Difficulty.HARDCORE)
                {
                    if (gameManager.LoadedGameData.missionsFailed >= 3 || gameManager.LoadedMissionData.isFinalMission)
                    {
                        // Call GameOver Permdadeath
                        gameManager.gameState.GameOver(GameOverEvent.PERMADEATH);
                    }
                }
                break;
            case MissionEndEvent.TUTORIAL_COMPLETE:
                Debug.Log($"Tutorial Successful!");
                gameManager.LoadedGameData.daysPassed++;

                // Invoke OnMissionEnd event
                OnMissionEnd?.Invoke(MissionEndEvent.TUTORIAL_COMPLETE);

                break;
            default:
                break;
        }
    }

    // Load a save and store in game manager loaded save
    public void LoadMission(int index, 
                            EscorteeScript escortee, 
                            WeaponScript meleeWeapon,
                            WeaponScript rangedWeapon1,
                            WeaponScript rangedWeapon2)
    {
        // Load mission
        gameManager.LoadedMissionData = gameManager.MissionDatas[index];

        // Assign weapons & escortee/vehicle
        gameManager.LoadedMissionData.vehicle = escortee;
        gameManager.LoadedMissionData.meleeWeapon = meleeWeapon;
        gameManager.LoadedMissionData.rangedWeapon1 = rangedWeapon1;
        gameManager.LoadedMissionData.rangedWeapon2 = rangedWeapon2;

        Debug.Log($"Loaded mission data at index = {index}");
    }
    // Load a save and store in game manager loaded save
    public void LoadMission(int index)
    {
        // Load mission
        gameManager.LoadedMissionData = gameManager.MissionDatas[index];

        // Assign weapons & escortee/vehicle
        gameManager.LoadedMissionData.vehicle = gameManager.gameEscortee.GetEscortee(gameManager.LoadedGameData.equippedVehicle);
        gameManager.LoadedMissionData.meleeWeapon = gameManager.gameWeapon.GetWeapon(gameManager.LoadedGameData.equippedMeleeWeapon);
        gameManager.LoadedMissionData.rangedWeapon1 = gameManager.gameWeapon.GetWeapon(gameManager.LoadedGameData.equippedRangedWeapon1);
        gameManager.LoadedMissionData.rangedWeapon2 = gameManager.gameWeapon.GetWeapon(gameManager.LoadedGameData.equippedRangedWeapon2);

        Debug.Log($"Loaded mission data at index = {index}");
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
                // Empty mission 1-3 first
                gameManager.MissionDatas[0].Empty();
                gameManager.MissionDatas[1].Empty();
                gameManager.MissionDatas[2].Empty();

                // Day 10 (Final Day)
                CreateMission(MissionDifficulty.FINAL, HazardRating.APOCALYPSE, 3);
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
                CreateMission(MissionDifficulty.FINAL, HazardRating.APOCALYPSE, 3);
        }

        Debug.Log($"Generated new mission set for day #{day}");
    }

    /// <summary>
    /// Create/generate a mission and store it in mission index
    /// <para><b>Note:</b> Use difficulty "FINAL" and hazard "APOCALYPSE" for final missions.</para>
    /// </summary>
    /// <param name="difficulty">The "difficulty" of the mission, determining which type of enemies can appear. Use this to introduce different enemy types at a certain stage of the game.</param>
    /// <param name="hazard">The hazard rating of the mission, determining the total amount of zombie to be spawned in a mission. Use this for mission variety options in a single day.</param>
    /// <param name="index">The index to store this mission in. Generally proportional with HazardRating, e.g 0 = NORMAL, 1 = INFESTED, 2 = OVERRUN</param>
    public void CreateMission(MissionDifficulty difficulty, HazardRating hazard, int index)
    {
        // Is this a final mission?
        bool isFinalMission = false;
        if (index == 3 && difficulty == MissionDifficulty.FINAL && hazard == HazardRating.APOCALYPSE)
            isFinalMission = true;

        // Randomize escort scene
        SceneName scene = escortScenes[Random.Range(0, escortScenes.Length)];

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
                enemies.Add(new Spawnable(gameManager.gameEnemy.gameManager.gameEnemy.GetEnemy(EnemyID.ZOMBIE).gameObject, 100));
                // Randomize escortee selection
                randomizeEscortee = Random.Range(0, 2) == 1 ? true : false;
                break;
            case MissionDifficulty.INTERMEDIATE:
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.ZOMBIE).gameObject, 80));
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.Z_BRUTE).gameObject, 20));
                // Randomize escortee selection
                randomizeEscortee = Random.Range(0, 2) == 1 ? true : false;
                break;
            case MissionDifficulty.HARD:
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.ZOMBIE).gameObject, 60));
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.Z_BRUTE).gameObject, 20));
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.Z_RUNNER).gameObject, 20));
                // Randomize escortee selection
                randomizeEscortee = Random.Range(0, 2) == 1 ? true : false;
                break;
            case MissionDifficulty.FINAL:
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.ZOMBIE).gameObject, 40));
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.Z_BRUTE).gameObject, 30));
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.Z_RUNNER).gameObject, 30));
                // NO RANDOM ESCORTEE FOR FINAL MISSION
                break;
            default:
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.ZOMBIE).gameObject, 60));
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.Z_BRUTE).gameObject, 20));
                enemies.Add(new Spawnable(gameManager.gameEnemy.GetEnemy(EnemyID.Z_RUNNER).gameObject, 20));
                // Randomize escortee selection
                randomizeEscortee = Random.Range(0, 2) == 1 ? true : false;
                break;
        }

        EscorteeScript vehicle;
        bool escorteeHasWeapon = false;
        if (randomizeEscortee)
        {
            // Choose random vehicle
            vehicle = gameManager.gameEscortee.EscorteePrefabs[Random.Range(0, gameManager.gameEscortee.EscorteePrefabs.Length)];

            // Randomize if escortee have weapons or not
            escorteeHasWeapon = Random.Range(0, 2) == 1 ? true : false;

            // Store mission to it's index
            gameManager.MissionDatas[index] = new MissionData(scene, vehicle, escorteeHasWeapon, zombieCount, baseReward, enemies, isFinalMission);
        }
        else
        {
            // Store mission to it's index
            gameManager.MissionDatas[index] = new MissionData(scene, escorteeHasWeapon, zombieCount, baseReward, enemies, isFinalMission);
        }
    }
}
