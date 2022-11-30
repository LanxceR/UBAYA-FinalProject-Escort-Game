using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayAudioManager : MonoBehaviour
{
    //Background Sounds and Music
    private FMOD.Studio.EventInstance backgroundAmbience;
    private FMOD.Studio.EventInstance backgroundMusic;

    private float playerHealth;

    //Buses
    private FMOD.Studio.Bus masterBus;

    private FMOD.Studio.Bus busAmbience;
    private FMOD.Studio.Bus busBlockade;
    private FMOD.Studio.Bus busConvoy;
    private FMOD.Studio.Bus busFootsteps;
    private FMOD.Studio.Bus busWeapon;
    private FMOD.Studio.Bus busZombie;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize Buses
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");

        busAmbience = FMODUnity.RuntimeManager.GetBus("bus:/SFX/Ambience");
        busBlockade = FMODUnity.RuntimeManager.GetBus("bus:/SFX/Blockade");
        busConvoy = FMODUnity.RuntimeManager.GetBus("bus:/SFX/Convoy");
        busFootsteps = FMODUnity.RuntimeManager.GetBus("bus:/SFX/Footsteps");
        busWeapon = FMODUnity.RuntimeManager.GetBus("bus:/SFX/Weapon");
        busZombie = FMODUnity.RuntimeManager.GetBus("bus:/SFX/Zombie");

        //Set bus initial volume (default it back to 100%)
        busAmbience.setVolume(1f);
        busBlockade.setVolume(1f);
        busConvoy.setVolume(1f);
        busFootsteps.setVolume(1f);
        busWeapon.setVolume(1f);
        busZombie.setVolume(1f);


        switch ((SceneName)GameManager.Instance.gameScene.GetCurrentScene().buildIndex)
        {
            //City
            case SceneName.MAP_CITY:
                backgroundAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/AmbienceCity");
                backgroundMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/GameplayLoop");
                break;
            //Suburbs
            case SceneName.MAP_SUBURBS:
                backgroundAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/AmbienceCity");
                backgroundMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/GameplayLoop");
                break;
            //Forest
            case SceneName.MAP_FOREST:
                backgroundAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/AmbienceForest");
                backgroundMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/GameplayLoop");
                break;
            //Main Hub
            case SceneName.MAIN_HUB:
                backgroundAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/AmbienceMenu");
                backgroundMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/MenuLoop");
                break;
        }

        backgroundAmbience.start();
        backgroundMusic.start();

        Debug.Log("BGM HAS STARTED");
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = GameManager.Instance.gamePlayer.ActivePlayer.healthScript.CurrentHealth * 10;

        backgroundMusic.setParameterByName("Health", playerHealth);

        //FILTERS BGM AUDIO AND LOWER AUDIO PLAYING IN BG
        if (GameManager.Instance.GameIsPlaying)
        {
            backgroundMusic.setParameterByName("Is Paused", 0);
            busAmbience.setVolume(1f);
            busConvoy.setVolume(1f);
        }
        else
        {
            backgroundMusic.setParameterByName("Is Paused", 1);
            busAmbience.setVolume(.4f);
            busConvoy.setVolume(0.4f);
        }
    }

    public void KillAll()
    {
        //During gameplay or when another scene is about to be called when this is called, ...
        //...kill all the sound events and lower volume of other sounds that are playing in the background.

        //This method will only be called during the end of a scene or when a mission has ended.

        //Gameplay sounds that needed to be stopped/lowered:
            //Ambience (stopped)
            //Blockade
            //Convoy
            //Footsteps
            //Music (stopped)
            //Weapon
            //Zombie

        //FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/");
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        //lower all bus volume by 80%
        busAmbience.setVolume(.2f);
        busBlockade.setVolume(.2f);
        busConvoy.setVolume(.2f);
        busFootsteps.setVolume(.2f);
        busWeapon.setVolume(.2f);
        busZombie.setVolume(.2f);


        //master bus volume set to 100% just in case
        masterBus.setVolume(1f);

        backgroundAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        backgroundMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
