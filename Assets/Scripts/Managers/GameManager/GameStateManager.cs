using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The game state manager script
/// </summary>
public class GameStateManager : MonoBehaviour
{
    // TODO: (DONE) Perform various gamestate checking for other non-time dependent actions in Unity!!!

    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;

    // TODO: Decide if UnityAction or UnityEvents are to be used here

    // Unity Events
    // Subbed at: InGamePauseUIScript
    /// <summary>
    /// Event invoked when pausing the game
    /// </summary>
    internal UnityAction OnPauseAction;
    // Subbed at: InGamePauseUIScript
    /// <summary>
    /// Event invoked when resuming the game
    /// </summary>
    internal UnityAction OnResumeAction;
    // Subbed at: InGameGameOverUIScript
    /// <summary>
    /// Event invoked when there's a game over
    /// </summary>
    internal UnityAction OnGameOver;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        // Add on death listeners to both active player and escortee
        if (gameManager.ActivePlayer)
            gameManager.ActivePlayer.healthScript.OnHealthReachedZero.AddListener(GameOver);

        if (gameManager.ActiveEscortee)
            gameManager.ActiveEscortee.healthScript.OnHealthReachedZero.AddListener(GameOver);
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        if (!gameManager.GameIsPlaying)
        {
            UpdateTimeScale(0f);
        } else
        {
            UpdateTimeScale(gameManager.GameTimeScale);
        }
    }

    private void GameOver()
    {
        // TODO: Implement GameOver events here
        Debug.Log("GAME OVER!");

        // Invoke OnGameOver event
        OnGameOver?.Invoke();
    }

    internal void UpdateTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    internal void StartGame()
    {
        gameManager.GameIsPlaying = true;
    }

    public void PauseGame()
    {
        gameManager.GameIsPlaying = false;
        Time.timeScale = 0f;
        OnPauseAction?.Invoke();
    }
    public void ResumeGame()
    {
        gameManager.GameIsPlaying = true;
        Time.timeScale = 1f;
        OnResumeAction?.Invoke();
    }
    public void PauseAndResumeGame()
    {
        if (gameManager.GameIsPlaying)
        {
            // Pause the game
            PauseGame();
        }
        else
        {
            // Resume the game
            ResumeGame();
        }
    }
}
