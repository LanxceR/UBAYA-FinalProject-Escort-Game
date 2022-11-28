using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUIScript : MonoBehaviour
{
    private FMOD.Studio.EventInstance musicEvent;
    private int hasClicked;

    private FMOD.Studio.VCA VcaMaster;
    private FMOD.Studio.VCA VcaSfx;
    private FMOD.Studio.VCA VcaBgm;

    // Start is called before the first frame update
    void Start()
    {
        //INITIALIZE VOLUME SOUNDS
        //Master
        VcaMaster = FMODUnity.RuntimeManager.GetVCA("vca:/Master");
        VcaMaster.setVolume(PlayerPrefs.GetFloat("Master"));
        //SFX
        VcaSfx = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
        VcaSfx.setVolume(PlayerPrefs.GetFloat("SFX"));
        //BGM
        VcaBgm = FMODUnity.RuntimeManager.GetVCA("vca:/BGM");
        VcaBgm.setVolume(PlayerPrefs.GetFloat("BGM"));

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
