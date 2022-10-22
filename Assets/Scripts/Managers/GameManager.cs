using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game manager script of the game. Also acts as a service locator for the other managers.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;
    internal static GameManager Instance
    {
        get
        {
            if (instance == null) Debug.LogError("GameManager is NULL");
            return instance;
        }
        private set { instance = value; }
    }

    [Header("Game States")]
    [SerializeField] 
    private bool gameIsPlaying = true; // Bool to determine if player is in menu or playing the game
    internal bool GameIsPlaying { get => gameIsPlaying; set => gameIsPlaying = value; }
    [SerializeField] [Range(0f, 2f)]
    private float gameTimeScale = 1f;
    public float GameTimeScale { get => gameTimeScale; 
        set 
        {
            gameTimeScale = value;
            gameState.UpdateTimeScale(gameTimeScale);
        } 
    }

    [Header("Player Prefabs")]
    [SerializeField] private GameObject playerPrefab; // Player prefab to spawn
    public GameObject PlayerPrefab { get => playerPrefab; set => playerPrefab = value; }
    [SerializeField] private GameObject activePlayer; // Stored active player
    internal GameObject ActivePlayer { get => activePlayer; set => activePlayer = value; }

    [Header("Player Datas")]
    [SerializeField] private PlayerData[] playerDatas = new PlayerData[3];
    public PlayerData[] PlayerDatas { get => playerDatas; 
        set
        {
            /** DEPRECATED
            // TODO: Move this somewhere else (for some reason this setter doesn't get executed after the first time) (Maybe debug more? Try changing the value on script rather than on inspector).
            // TODO: Also figure out proper implementation for identifying save slot number
            for (int i = 0; i < playerDatas.Length; i++)
            {
                if (!playerDatas[i].Equals(value[i]))
                {
                    playerDatas[i] = value[i];
                    gameData.SaveGame($"savegame_{i}", value[i]);
                }
            }
            */
            playerDatas = value;
        }
    }
    [SerializeField] private PlayerData loadedPlayerData;
    internal PlayerData LoadedPlayerData { get => loadedPlayerData; set => loadedPlayerData = value; }

    [Header("Service Locators (Other managers)")]
    // TODO: Put other managers here. GameManager is going to act as the main entryway for accessing these managers
    [SerializeField]
    internal GameStateManager gameState;
    [SerializeField]
    internal GameInputManager gameInput;
    [SerializeField]
    internal GameDataManager gameData;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Debug.Log("GameManager script awake");

        if (instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        // TODO: Implement the rest of the saving and loading system of the game (autosave, creation, deletion, etc)
        PlayerDatas = gameData.LoadGamesFromFiles();
    }

    public void GameOver(GameObject killer)
    {
        // TODO: Implement GameOver events here
    }
}
