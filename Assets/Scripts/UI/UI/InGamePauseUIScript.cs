using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The pause menu UI script (handles the pause menu UI)
/// </summary>
public class InGamePauseUIScript : MonoBehaviour
{
    // Reference to the main UI script
    [SerializeField]
    private InGameUIScript inGameUIScript;

    // Components
    [SerializeField]
    private GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("InGamePauseUIScript starting");

        // Subscribe methods to game manager
        GameManager.Instance.gameState.OnPauseAction += PauseGame;
        GameManager.Instance.gameState.OnResumeAction += ResumeGame;

        // Disable pause panel at start
        pausePanel.SetActive(false);
    }

    // Methods to invoke when pausing and resuming the game
    private void PauseGame()
    {
        // Enable pause panel
        pausePanel.SetActive(true);
    }
    private void ResumeGame()
    {
        // Disable pause panel
        pausePanel.SetActive(false);
    }
}
