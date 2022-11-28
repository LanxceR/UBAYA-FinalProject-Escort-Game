using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneSkipScript : MonoBehaviour
{
    float timer;
    float holdDur = 1f;
    [SerializeField]
    TextMeshProUGUI text;

    void Start()
    {
        if(PlayerPrefs.GetFloat("CanSkip") == 0)
        {
            text.enabled = false;
        }
        else
        {
            text.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetFloat("CanSkip") == 1)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                timer = Time.time;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                if (Time.time - timer > holdDur)
                {
                    timer = float.PositiveInfinity;

                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
                    GameManager.Instance.gameScene.GotoScene(SceneName.MAIN_HUB);
                }
            }
            else
            {
                timer = float.PositiveInfinity;
            }
        }
    }
}
