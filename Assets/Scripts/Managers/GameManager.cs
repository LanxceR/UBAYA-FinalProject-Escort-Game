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
    [SerializeField] private GameObject activePlayer; // Stored active player
    public GameObject PlayerPrefab { get => playerPrefab; set => playerPrefab = value; }
    internal GameObject ActivePlayer { get => activePlayer; set => activePlayer = value; }

    [Header("Service Locators (Other managers)")]
    // TODO: Put other managers here. GameManager is going to act as the main entryway for accessing these managers
    [SerializeField]
    internal GameStateManager gameState;
    [SerializeField]
    internal GameInputManager gameInput;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Debug.Log("GameManager script awake");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    public void GameOver(GameObject killer)
    {
        // TODO: Implement GameOver events here
    }
}
