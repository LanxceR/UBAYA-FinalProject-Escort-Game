using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game data manager script
/// </summary>
public class GameDataManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    public GameManager gameManager;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        // TODO: (WIP) Implement the rest of the saving and loading system of the game (autosave, creation, deletion, etc)
        gameManager.GameDatas = LoadGamesFromFiles();
        LoadGame(0);
    }

    // Create a new save
    public void CreateGame(
        int index,
        Difficulty difficulty,
        float money,
        int daysPassed,
        int missionsCompleted,
        int missionsFailed,
        float ammo_LIGHT, float ammo_SHOTGUN, float ammo_HEAVY,
        List<WeaponID> ownedWeapons, List<EscorteeID> ownedVehicles,
        WeaponID equippedMeleeWeapon,
        WeaponID equippedRangedWeapon1,
        WeaponID equippedRangedWeapon2,
        EscorteeID equippedVehicle
        )
    {
        // Create new player data and store in game manager array
        gameManager.GameDatas[index] = new PlayerData(
            index, 
            difficulty, 
            money, 
            daysPassed, 
            missionsCompleted, 
            missionsFailed, 
            ammo_LIGHT, ammo_SHOTGUN, ammo_HEAVY,
            ownedWeapons, ownedVehicles,
            equippedMeleeWeapon,
            equippedRangedWeapon1,
            equippedRangedWeapon2,
            equippedVehicle
            );
    }
    public void CreateGame(
        int index,
        Difficulty difficulty
        )
    {
        List<WeaponID> startingWeapon = new List<WeaponID>();
        startingWeapon.Add(WeaponID.PIPE);

        List<EscorteeID> startingVehicle = new List<EscorteeID>();
        startingVehicle.Add(EscorteeID.BUS);

        // Create new player data and store in game manager array
        gameManager.GameDatas[index] = new PlayerData(
            index,
            difficulty,
            0,
            0,
            0,
            0,
            0, 0, 0,
            startingWeapon, startingVehicle,
            WeaponID.PIPE,
            WeaponID.NONE,
            WeaponID.NONE,
            EscorteeID.BUS
            );
    }

    // Save the game
    /// <summary>
    /// Store LoadedGameData into GameDatas[index], and then write to savefile
    /// </summary>
    public void SaveGame()
    {
        // Fetch loaded player data
        PlayerData data = gameManager.LoadedGameData;
        // Store data in game manager player datas array
        gameManager.GameDatas[data.index] = data;
        // Save loaded player data
        SaveSystem.SaveGame($"savegame_{data.index}", data);
    }

    // Load a save and store in game manager loaded save
    public void LoadGame(int index)
    {
        gameManager.LoadedGameData = gameManager.GameDatas[index];

        if (gameManager.gameWeapon) gameManager.gameWeapon.UpdateAllWeaponFlags();
        if (gameManager.gameEscortee) gameManager.gameEscortee.UpdateAllEscorteeFlags();

        Debug.Log($"Loaded stored player data at index = {index}");
    }

    // Unload the game manager loaded save
    public void UnloadGame()
    {
        gameManager.LoadedGameData.Empty();
    }

    // Delete (and unload) current loaded save
    internal void DeleteSave()
    {
        // Fetch loaded player data
        PlayerData data = gameManager.LoadedGameData;
        // Empty that data
        gameManager.GameDatas[data.index].Empty();
        data.Empty();
        // Save the empty data slot
        SaveGame();
    }

    // Save all games / data
    public void SaveGamesToFiles()
    {
        Debug.Log($"Loading all save files...");
        PlayerData[] loadedDatas = LoadGamesFromFiles();
        for (int i = 0; i < loadedDatas.Length; i++)
        {
            gameManager.GameDatas[i].index = i;
            if (!gameManager.GameDatas[i].Equals(loadedDatas[i]))
            {
                SaveSystem.SaveGame($"savegame_{i}", gameManager.GameDatas[i]);
                continue;
            }

            Debug.Log($"The exact same savegame_{i} already exists, skipping...");
        }
    }

    // Load all games / data
    public PlayerData[] LoadGamesFromFiles()
    {
        PlayerData[] datas = new PlayerData[gameManager.GameDatas.Length];
        for (int i = 0; i < gameManager.GameDatas.Length; i++)
        {
            datas[i] = SaveSystem.LoadGame($"savegame_{i}");
            if (datas[i] != null) datas[i].index = i;
        }

        return datas;
    }
}
