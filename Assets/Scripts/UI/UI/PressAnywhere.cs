using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnywhere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Script started");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("Button has been pressed");

            GameManager.Instance.gameScene.GotoScene(SceneName.MAIN_HUB);
        }
    }
}
