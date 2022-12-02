using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugMissionEnd : MonoBehaviour
{
    [SerializeField]
    internal SceneName sceneTarget = SceneName.MAIN_HUB;
    [SerializeField]
    private Button btnContinue;
    [SerializeField]
    private TextMeshProUGUI btnText;

    public void GoToSceneTarget()
    {
        GameManager.Instance.gameScene.GotoScene(sceneTarget);
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneTarget = SceneName.MAIN_HUB;
        btnContinue.gameObject.SetActive(false);

        GameManager.Instance.gameState.OnGameOver += GameOver;
        GameManager.Instance.gameMission.OnMissionEnd += MissionEnd;
    }

    void GameOver(GameOverEvent gameOverEvent)
    {
        // If game over, change sceneTarget
        switch (gameOverEvent)
        {
            case GameOverEvent.PERMADEATH:
                sceneTarget = SceneName.PERMADEATH_SCREEN;
                btnText.text = $"Go to {sceneTarget.ToString()}";
                break;
            case GameOverEvent.ENDING:
                sceneTarget = SceneName.ENDING_SCREEN;
                btnText.text = $"Go to {sceneTarget.ToString()}";
                break;
            default:
                break;
        }
    }

    private void MissionEnd(MissionEndEvent missionEndEvent, float reward)
    {
        btnContinue.gameObject.SetActive(true);
    }

    // This function is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        GameManager.Instance.gameState.OnGameOver -= GameOver;
        GameManager.Instance.gameMission.OnMissionEnd -= MissionEnd;
    }
}
