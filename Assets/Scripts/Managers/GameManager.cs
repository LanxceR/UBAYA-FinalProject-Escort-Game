using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game manager script of the game. Also acts as a service locator for the other managers.
/// </summary>
public class GameManager : MonoBehaviour
{
    // TODO: Enemy AI, spawning, etc
    // TODO: Procgenerated map

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
    [SerializeField] private PlayerScript playerPrefab; // Player prefab to spawn
    public PlayerScript PlayerPrefab { get => playerPrefab; set => playerPrefab = value; }
    [SerializeField] private PlayerScript activePlayer; // Stored active player
    internal PlayerScript ActivePlayer { get => activePlayer; 
        set
        {
            activePlayer = value;
            UI.HUDScript.hudAmmoScript.AssignWeaponScript();
        }
    }

    [Header("Escortee Prefabs")]
    [SerializeField] private EscorteeScript escorteePrefab; // Escortee prefab to spawn
    public EscorteeScript EscorteePrefab { get => escorteePrefab; set => escorteePrefab = value; }
    [SerializeField] private EscorteeScript activeEscortee; // Stored active escortee
    internal EscorteeScript ActiveEscortee { get => activeEscortee; set => activeEscortee = value; }

    [Header("Game Datas")]
    [SerializeField] private PlayerData[] gameDatas = new PlayerData[3];
    public PlayerData[] GameDatas { get => gameDatas; 
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
            gameDatas = value;
        }
    }
    [SerializeField] private PlayerData loadedGameData;
    public PlayerData LoadedGameData { get => loadedGameData; set => loadedGameData = value; }


    [Header("Cameras Prefab")]
    [SerializeField] private CameraManager camerasPrefab;
    [SerializeField] private CameraManager activeCameras;
    public CameraManager Cameras { get => activeCameras; private set => activeCameras = value; }

    [Header("UI Prefab")]
    [SerializeField] private UIManager uiPrefab;
    [SerializeField] private UIManager activeUI;
    public UIManager UI { get => activeUI; private set => activeUI = value; }


    [Header("Service Locators (Other managers)")]
    // TODO: Put other managers here. GameManager is going to act as the main entryway for accessing these managers
    [SerializeField]
    internal GameStateManager gameState;
    [SerializeField]
    internal GameInputManager gameInput;
    [SerializeField]
    internal GameDataManager gameData;
    [SerializeField]
    internal GamePlayerManager gamePlayer;
    [SerializeField]
    internal GameEscorteeManager gameEscortee;

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

        FindActiveCameras();
        FindActiveUI();
    }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        InitializeCameras();
        InitializeUI();
    }

    // Find an active Cameras object in hirearchy
    private void FindActiveCameras()
    {
        var activeCameras = FindObjectOfType<CameraManager>();

        if (activeCameras)
        {
            Cameras = activeCameras;
        }
    }
    // Find an active UI object in hirearchy
    private void FindActiveUI()
    {
        var activeUI = FindObjectOfType<UIManager>();

        if (activeUI)
        {
            UI = activeUI;
        }
    }

    // Initialize Cameras
    public void InitializeCameras()
    {
        InitializeCameras(camerasPrefab.transform);
    }
    public void InitializeCameras(Transform spawnPoint)
    {
        if (!Cameras)
        {
            Cameras = Instantiate(camerasPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Cameras.gameObject.SetActive(true);
            Cameras.transform.position = spawnPoint.position;
        }
    }

    // Initialize UI
    public void InitializeUI()
    {
        InitializeUI(uiPrefab.transform);
    }
    public void InitializeUI(Transform spawnPoint)
    {
        if (!UI)
        {
            UI = Instantiate(uiPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            UI.gameObject.SetActive(true);
            UI.transform.position = spawnPoint.position;
        }
    }
}
