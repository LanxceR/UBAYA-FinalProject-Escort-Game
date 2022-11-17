using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game mission manager script
/// </summary>
public enum MissionDifficulty { EASY, INTERMEDIATE, HARD}
public class GameMissionManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void GenerateMission(MissionDifficulty difficulty, HazardRating hazard, int index)
    {
        // TODO: Finish mission mechanics

        // Choose random vehicle
        EscorteeScript vehicle = gameManager.EscorteePrefabs[Random.Range(0, gameManager.EscorteePrefabs.Length)];

        // Randomize if escortee have weapons or not
        bool escorteeHasWeapon = Random.Range(0, 2) == 1 ? true : false;

        // Randomize Zombie Count
        int zombieCount;
        switch (hazard)
        {
            case HazardRating.NORMAL:
                zombieCount = Random.Range(25, 51);
                break;
            case HazardRating.INFESTED:
                zombieCount = Random.Range(50, 101);
                break;
            case HazardRating.OVERRUN:
                zombieCount = Random.Range(100, 151);
                break;
            default:
                zombieCount = Random.Range(50, 101);
                break;
        }

        // Randomize Zombie Types
        switch (difficulty)
        {
            case MissionDifficulty.EASY:
                break;
            case MissionDifficulty.INTERMEDIATE:
                break;
            case MissionDifficulty.HARD:
                break;
            default:
                break;
        }
    }
}
