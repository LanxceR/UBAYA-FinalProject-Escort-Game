using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The game over menu UI script (handles the game over menu UI)
/// </summary>
public class InGameGameOverUIScript : MonoBehaviour
{
    // Reference to the main UI script
    [SerializeField]
    private InGameUIScript inGameUIScript;

    // Components
    [SerializeField]
    private GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("InGameGameOverUIScript starting");

        GameManager.Instance.gameState.OnGameOver += GameOver;
    }

    // Methods
    private void GameOver()
    {
        // Enable pause panel
        gameOverPanel.SetActive(true);
    }
    private void DisableGameOverPanel()
    {
        // Enable pause panel
        gameOverPanel.SetActive(false);
    }
}
