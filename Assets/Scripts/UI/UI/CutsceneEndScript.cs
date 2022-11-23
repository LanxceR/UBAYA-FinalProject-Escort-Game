using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEndScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.gameScene.GotoScene(SceneName.MAIN_HUB);
    }
}
