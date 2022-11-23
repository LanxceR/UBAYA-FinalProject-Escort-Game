using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The game over menu UI script (handles the game over menu UI)
/// </summary>
public class InGameMissionEndUIScript : MonoBehaviour
{
    // Reference to the main UI script
    [SerializeField]
    private InGameUIScript inGameUIScript;

    // Components
    [SerializeField]
    private GameObject missionSuccessPanel;
    [SerializeField]
    private GameObject missionFailedPanel;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("InGameGameOverUIScript starting");

        GameManager.Instance.gameState.OnGameOver += GameOver;
    }

    // Methods
    private void GameOver(GameOverEvent gameOverEvent)
    {
        switch (gameOverEvent)
        {
            case GameOverEvent.MISSION_SUCCESS:
                missionSuccessPanel.SetActive(true);
                break;
            case GameOverEvent.MISSION_FAILED:
                missionFailedPanel.SetActive(true);
                break;
            default:
                break;
        }
    }
    private void DisableGameOverPanel()
    {
        // Enable pause panel
        missionFailedPanel.SetActive(false);
    }
}