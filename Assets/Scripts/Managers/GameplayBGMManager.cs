using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayBGMManager : MonoBehaviour
{
    private FMOD.Studio.EventInstance backgroundAmbience;
    private FMOD.Studio.EventInstance backgroundMusic;

    private float playerHealth;
    private FMOD.Studio.Bus bus;

    // Start is called before the first frame update
    void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/SFX/Convoy");

        backgroundAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/AmbienceCity");
        backgroundAmbience.start();

        backgroundMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/GameplayLoop");
        backgroundMusic.start();

        Debug.Log("BGM HAS STARTED");
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = GameManager.Instance.gamePlayer.ActivePlayer.healthScript.CurrentHealth * 10;

        backgroundMusic.setParameterByName("Health", playerHealth);
        backgroundMusic.getParameterByName("Health", out float health);

        //FILTERS BGM AUDIO
        //if (GameManager.Instance.gameState.gameManager.GameIsPlaying)
        if (GameManager.Instance.GameIsPlaying)
        {
            backgroundMusic.setParameterByName("Is Paused", 0);

            bus.setVolume(1f);
        }
        else
        {
            backgroundMusic.setParameterByName("Is Paused", 1);

            bus.setVolume(0.4f);
        }

        //Debug.Log("GameplayBGMManager, Current health parameter in FMOD: " + health + "\n Playerhealth: " + playerHealth);
    }
}
