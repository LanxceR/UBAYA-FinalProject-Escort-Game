using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToScene : MonoBehaviour
{
    [SerializeField]
    internal SceneName sceneTarget;

    public void GoToSceneTarget()
    {
        GameManager.Instance.gameScene.GotoScene(sceneTarget);
    }
}
