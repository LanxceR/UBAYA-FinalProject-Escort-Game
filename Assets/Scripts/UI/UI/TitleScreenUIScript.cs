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
        if (PlayerPrefs.HasKey("Master"))
        {
            VcaMaster.setVolume(PlayerPrefs.GetFloat("Master"));
        }
        else
        {
            VcaMaster.setVolume(1f);
        }

        //SFX
        VcaSfx = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
        if (PlayerPrefs.HasKey("SFX"))
        {
            VcaSfx.setVolume(PlayerPrefs.GetFloat("SFX"));
        }
        else
        {
            VcaSfx.setVolume(1f);
        }

        //BGM
        VcaBgm = FMODUnity.RuntimeManager.GetVCA("vca:/BGM");
        if (PlayerPrefs.HasKey("BGM"))
        {
            VcaBgm.setVolume(PlayerPrefs.GetFloat("BGM"));
        }
        else
        {
            VcaBgm.setVolume(1f);
        }


        //Make sure that master bus volume is 100%
        FMOD.Studio.Bus MasterBus;
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        MasterBus.setVolume(1f);

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
