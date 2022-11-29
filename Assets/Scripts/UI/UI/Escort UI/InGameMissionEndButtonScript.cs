using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameMissionEndButtonScript : MonoBehaviour
{
    [SerializeField]
    internal SceneName sceneTarget = SceneName.MAIN_HUB;
    [SerializeField]
    private Button btnContinue;
    [SerializeField]
    private TextMeshProUGUI btnText;

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
                sceneTarget = SceneName.TEST_PERMADEATH_SCREEN;
                //btnText.text = $"Go to {sceneTarget.ToString()}";
                btnText.text = "End game";
                break;
            case GameOverEvent.ENDING:
                sceneTarget = SceneName.TEST_ENDING_SCREEN;
                //btnText.text = $"Go to {sceneTarget.ToString()}";
                btnText.text = "Continue";
                break;
            default:
                break;
        }
    }

    private void MissionEnd(MissionEndEvent missionEndEvent, float reward)
    {
        btnContinue.gameObject.SetActive(true);
    }
    public void GoToSceneTarget()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        GameManager.Instance.gameScene.GotoScene(sceneTarget);
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }
}
