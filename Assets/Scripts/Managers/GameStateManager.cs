using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The game state manager script
/// </summary>
public class GameStateManager : MonoBehaviour
{
    // TODO: Perform various gamestate checking for other non-time dependent actions in Unity!!!

    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;

    // Unity Events
    // Subbed at:
    internal UnityAction OnPauseAction;
    // Subbed at:
    internal UnityAction OnResumeAction;

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        if (!gameManager.GameIsPlaying)
        {
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
        }
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
