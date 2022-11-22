using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameOverEvent
{
    PERMADEATH,
    MISSION_SUCCESS,
    MISSION_FAILED,
    ENDING
}

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
    internal UnityAction<GameOverEvent> OnGameOver;

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        // State Update
        if (!gameManager.GameIsPlaying)
        {
            UpdateTimeScale(0f);
        } else
        {
            UpdateTimeScale(gameManager.GameTimeScale);
        }
    }

    public void GameOver(GameOverEvent gameOverEvent)
    {        
        // TODO: Move the PERMADEATH and ENDING event to somewhere else (for better mission conclusion screen and then jumping into an end scene)
        switch (gameOverEvent)
        {
            case GameOverEvent.MISSION_SUCCESS:
                Debug.Log($"Mission Successful!");
                gameManager.LoadedGameData.missionsCompleted++;
                if (gameManager.LoadedMissionData.isFinalMission)
                {
                    // Call GameOver again, but with an ending
                    GameOver(GameOverEvent.ENDING); 
                }
                break;
            case GameOverEvent.MISSION_FAILED:
                Debug.Log($"Mission Failed!");
                gameManager.LoadedGameData.missionsFailed++;
                if (gameManager.LoadedGameData.difficulty == Difficulty.HARDCORE)
                {
                    if (gameManager.LoadedGameData.missionsFailed >= 3 || gameManager.LoadedMissionData.isFinalMission)
                    {
                        // Call GameOver again, but permadeath
                        GameOver(GameOverEvent.PERMADEATH);
                    }
                }
                break;
            case GameOverEvent.PERMADEATH:
                Debug.Log($"GAME OVER!");
                break;
            case GameOverEvent.ENDING:
                Debug.Log($"YOU WON THE GAME!");
                break;
            default:
                break;
        }

        // Invoke OnGameOver event
        OnGameOver?.Invoke(gameOverEvent);
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
