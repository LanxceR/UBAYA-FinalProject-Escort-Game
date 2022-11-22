using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game manager script of the game. Also acts as a service locator for the other managers.
/// </summary>
public class GameManager : MonoBehaviour
{
    // TODO: Enemy AI, spawning, etc
    // TODO: Try storing ALL prefabs in a ScriptableObject or other methods
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
    [SerializeField] [Range(0f, 2f)]
    private float gameTimeScale = 1f;
    internal bool GameIsPlaying { get => gameIsPlaying; set => gameIsPlaying = value; }
    public float GameTimeScale { get => gameTimeScale; set => gameTimeScale = value; }
    

    [Header("Game Datas")]
    [SerializeField] private PlayerData[] gameDatas = new PlayerData[3];
    [SerializeField] private PlayerData loadedGameData;
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
    public PlayerData LoadedGameData { get => loadedGameData; set => loadedGameData = value; }


    [Header("Mission Datas")]
    [SerializeField] private MissionData[] missionDatas = new MissionData[3];
    [SerializeField] private MissionData loadedMissionData;
    public MissionData[] MissionDatas { get => missionDatas; set => missionDatas = value; }
    public MissionData LoadedMissionData { get => loadedMissionData; set => loadedMissionData = value; }


    [Header("Cameras Prefab")]
    [SerializeField] private EscortCameraManager inGameCamerasPrefab;
    [SerializeField] private EscortCameraManager inGameActiveCameras;
    public EscortCameraManager InGameCameras { get => inGameActiveCameras; private set => inGameActiveCameras = value; }

    [Header("UI Prefab")]
    [SerializeField] private UIManager inGameUIPrefab;
    [SerializeField] private UIManager inGameActiveUI;
    public UIManager InGameUI { get => inGameActiveUI; private set => inGameActiveUI = value; }


    [Header("Service Locators (Other managers)")]
    // TODO: Put other managers here. GameManager is going to act as the main entryway for accessing these managers
    [SerializeField]
    internal GameSceneManager gameScene;
    [SerializeField]
    internal GameStateManager gameState;
    [SerializeField]
    internal GameInputManager gameInput;
    [SerializeField]
    internal GameDataManager gameData;
    [SerializeField]
    internal GameMissionManager gameMission;
    [SerializeField]
    internal GamePlayerManager gamePlayer;
    [SerializeField]
    internal GameEscorteeManager gameEscortee;
    [SerializeField]
    internal GameWeaponManager gameWeapon;
    [SerializeField]
    internal GameEnemyManager gameEnemy;

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
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        // Debugging
    }

    // Find an active In-Game Cameras object in hirearchy
    public void FindActiveInGameCameras()
    {
        var activeCameras = FindObjectOfType<EscortCameraManager>();

        if (activeCameras)
        {
            InGameCameras = activeCameras;
        }
    }
    // Find an active In-Game UI object in hirearchy
    public void FindActiveInGameUI()
    {
        var activeUI = FindObjectOfType<UIManager>();

        if (activeUI)
        {
            InGameUI = activeUI;
        }
    }

    // Initialize In-Game Cameras
    public void TryInitializeInGameCameras()
    {
        TryInitializeInGameCameras(inGameCamerasPrefab.transform);
    }
    public void TryInitializeInGameCameras(Transform spawnPoint)
    {
        if (!InGameCameras)
        {
            InGameCameras = Instantiate(inGameCamerasPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            InGameCameras.gameObject.SetActive(true);
            InGameCameras.transform.position = spawnPoint.position;
        }
    }

    // Initialize In-Game UI
    public void TryInitializeInGameUI()
    {
        TryInitializeInGameUI(inGameUIPrefab.transform);
    }
    public void TryInitializeInGameUI(Transform spawnPoint)
    {
        if (!InGameUI)
        {
            InGameUI = Instantiate(inGameUIPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            InGameUI.gameObject.SetActive(true);
            InGameUI.transform.position = spawnPoint.position;
        }
    }
}
