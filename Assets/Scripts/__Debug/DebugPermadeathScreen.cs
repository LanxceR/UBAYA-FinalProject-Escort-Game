using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPermadeathScreen : MonoBehaviour
{
    public void GameOver()
    {
        // Delete Save
        GameManager.Instance.gameData.DeleteSave();

        // Go back to title screen
        GameManager.Instance.gameScene.GotoScene(SceneName.TITLE_SCREEN);
    }
}
