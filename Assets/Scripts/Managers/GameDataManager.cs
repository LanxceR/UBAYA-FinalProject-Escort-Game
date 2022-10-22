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

    // Create a new save
    internal void CreateGame(
        int index,
        Difficulty difficulty,
        float money,
        int daysPassed,
        int missionsCompleted,
        int missionsFailed
        )
    {
        // Create new player data and store in game manager array
        gameManager.PlayerDatas[index] = new PlayerData(index, difficulty, money, daysPassed, missionsCompleted, missionsFailed);
    }

    // Save the game
    internal void SaveGame()
    {
        // Fetch loaded player data
        PlayerData data = gameManager.LoadedPlayerData;
        // Store data in game manager player datas array
        gameManager.PlayerDatas[data.index] = data;
        // Save loaded player data
        SaveSystem.SaveGame($"savegame_{data.index}", data);
    }

    // Load a save and store in game manager loaded save
    internal void LoadGame(int index)
    {
        gameManager.LoadedPlayerData = gameManager.PlayerDatas[index];
    }

    // Unload the game manager loaded save
    internal void UnloadGame()
    {
        gameManager.LoadedPlayerData.Empty();
    }

    // Delete (and unload) current loaded save
    internal void DeleteSave()
    {
        // Fetch loaded player data
        PlayerData data = gameManager.LoadedPlayerData;
        // Empty that data
        gameManager.PlayerDatas[data.index].Empty();
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
            gameManager.PlayerDatas[i].index = i;
            if (!gameManager.PlayerDatas[i].Equals(loadedDatas[i]))
            {
                SaveSystem.SaveGame($"savegame_{i}", gameManager.PlayerDatas[i]);
                continue;
            }

            Debug.Log($"The exact same savegame_{i} already exists, skipping...");
        }
    }

    // Load all games / data
    public PlayerData[] LoadGamesFromFiles()
    {
        PlayerData[] datas = new PlayerData[gameManager.PlayerDatas.Length];
        for (int i = 0; i < gameManager.PlayerDatas.Length; i++)
        {
            datas[i] = SaveSystem.LoadGame($"savegame_{i}");
            if (datas[i] != null) datas[i].index = i;
        }

        return datas;
    }
}
