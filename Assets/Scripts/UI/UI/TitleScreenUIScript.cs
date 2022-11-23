using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUIScript : MonoBehaviour
{
    private FMOD.Studio.EventInstance musicEvent;
    private int hasClicked;

    // Start is called before the first frame update
    void Start()
    {
        hasClicked = 0;
        Debug.Log("Script started");
        musicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Music/IntroLoop");
        musicEvent.start();
    }

    // Update is called once per frame
    void Update()
    {
        Click();
    }

    void Click()
    {
        if(hasClicked == 0)
        {
            if (Input.anyKeyDown)
            {
                Debug.Log("Button has been pressed");
                musicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                musicEvent.release();

                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/ClickDiegetic");
                hasClicked = 1;

                GameManager.Instance.gameScene.GotoScene(SceneName.SAVE_LOAD);
            }
        }
    }
}
