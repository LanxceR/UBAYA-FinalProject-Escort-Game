using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEndingScreen : MonoBehaviour
{
    public void GoBackToMenu()
    {
        GameManager.Instance.gameScene.GotoScene(SceneName.MAIN_HUB);
    }
}
