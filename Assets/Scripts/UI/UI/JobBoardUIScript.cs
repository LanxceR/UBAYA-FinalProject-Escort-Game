using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JobBoardUIScript : MonoBehaviour
{
    public GameObject briefingUI;
    private int missionIndex;

    [Header("Briefing UI GameObjects")]
    public TextMeshProUGUI missionLocation;
    public TextMeshProUGUI missionDescription;
    public Image missionImage;
    public TextMeshProUGUI missionReward;
    public TextMeshProUGUI missionEscorteeChoice;
    public TextMeshProUGUI missionWeaponChoice;

    [Header("Pages")]
    public GameObject job1;
    public GameObject job2;
    public GameObject job3;
    public GameObject jobFinal;

    [Header("UI Panels & Button Control")]
    public GameObject convoySelection;
    public Button convoySelectionButton;
    public GameObject weaponSelection;

    private FMOD.Studio.EventInstance instance;

    // Start is called before the first frame update

    //This method will check for all available missions.
    //Mission objects will be hidden depending on the status of the game.
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/AmbienceLight");
        instance.start();

        if (GameManager.Instance.LoadedGameData.difficulty == Difficulty.HARDCORE)
        {
            if(GameManager.Instance.LoadedGameData.daysPassed == 10)
            {
                job1.SetActive(false);
                job2.SetActive(false);
                job3.SetActive(false);
                jobFinal.SetActive(true);
            }
            else
            {
                job1.SetActive(true);
                job2.SetActive(true);
                job3.SetActive(true);
                jobFinal.SetActive(false);
            }
        }
        else
        {
            if(GameManager.Instance.LoadedGameData.missionsCompleted >= 5)
            {
                job1.SetActive(true);
                job2.SetActive(true);
                job3.SetActive(true);
                jobFinal.SetActive(true);
            }
            else
            {
                job1.SetActive(true);
                job2.SetActive(true);
                job3.SetActive(true);
                jobFinal.SetActive(false);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            instance.start();
        }
    }

    public void OpenBriefing(int i)
    {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        missionIndex = i;

        Debug.Log("Mission number: " + (i + (int)1));
        briefingUI.SetActive(true);

        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            g.GetComponent<PolygonCollider2D>().enabled = false;
        }

        Refresh();
    }

    //This method will load or refresh the initial data to the briefingUI texts in accordance with generated mission
    void Refresh()
    {
        //CHECK MISSION LOCATION
        switch (missionIndex)
        {
            case 0:
                missionImage.sprite = job1.GetComponent<SpriteRenderer>().sprite;
                break;
            case 1:
                missionImage.sprite = job2.GetComponent<SpriteRenderer>().sprite;
                break;
            case 2:
                missionImage.sprite = job3.GetComponent<SpriteRenderer>().sprite;
                break;
            case 3:
                missionImage.sprite = jobFinal.GetComponent<SpriteRenderer>().sprite;
                break;
        }
        missionLocation.text = GameManager.Instance.MissionDatas[missionIndex].escortScene.ToString().Replace('_', ' ');


        //Description Text
        //Two parts:
        //  1 = Message based on current city
        //  2 = Message based on difficulty
        //Concat
        switch (GameManager.Instance.MissionDatas[missionIndex].escortScene)
        {
            case SceneName.TEST_ESCORT_SCENE:
                missionDescription.text = "Supplies has been retrieved from the city, and needs to be delivered back to the base.";
                break;
        }

        switch (GameManager.Instance.MissionDatas[missionIndex].baseReward)
        {
            case 1000:
                missionDescription.text += " Expect to run into some zombies.";
                break;
            case 1500:
                missionDescription.text += " Beware, the location is heavily infested.";
                break;
            case 2000:
                missionDescription.text += " The location is swarming with the undead. Be cautious.";
                break;
            case 3000:
                missionDescription.text += " Try to make it out alive.";
                break;
        }

        //BUTTON CONVOY SELECTION
        //CHECK IF A SPECIFIC VEHICLE IS REQUIRED
        if (GameManager.Instance.MissionDatas[missionIndex].vehicle == null)
        {
            convoySelectionButton.interactable = true;
            missionEscorteeChoice.text = GameManager.Instance.LoadedGameData.equippedVehicle.ToString().Replace('_', ' ');
            convoySelectionButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            convoySelectionButton.interactable = false;
            missionEscorteeChoice.text = GameManager.Instance.MissionDatas[missionIndex].vehicle.id.ToString().Replace('_', ' ') + " (REQUIRED)";
            convoySelectionButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
        }

        missionReward.text = "$" + GameManager.Instance.MissionDatas[missionIndex].baseReward;
        missionWeaponChoice.text = GameManager.Instance.LoadedGameData.equippedRangedWeapon1.ToString().Replace('_', ' ') + "/"
            + GameManager.Instance.LoadedGameData.equippedRangedWeapon2.ToString().Replace('_', ' ') + "/"
            + GameManager.Instance.LoadedGameData.equippedMeleeWeapon.ToString().Replace('_', ' ');

    }

    public void CloseBriefing()
    {
        //OLD CODE WILL ENABLE EVERY SINGLE DIEGETIC OBJECT IN THE SCENE
        //NEW CODE WILL ONLY ENABLE DIEGETIC OBJECS IN THE CURRENT PARENT
        /*GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            g.GetComponent<PolygonCollider2D>().enabled = true;
        }*/

        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            bool value = child.CompareTag("Diegetic");
            if (value == true)
            {
                child.GetComponent<PolygonCollider2D>().enabled = true;
            }
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        instance.start();

        briefingUI.SetActive(false);
    }

    public void CloseJobBoardUI()
    {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            //g.GetComponent<HubMenuUI>().enabled = true;
            g.GetComponent<PolygonCollider2D>().enabled = true;
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        this.gameObject.SetActive(false);
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    public void OpenBriefingConvoy()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/ClickDiegetic");
        convoySelection.SetActive(true);
    }

    public void CloseBriefingConvoy()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        convoySelection.SetActive(false);
        Refresh();
    }

    public void OpenBriefingWeapon()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/ClickDiegetic");
        weaponSelection.SetActive(true);
    }

    public void CloseBriefingWeapon()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        weaponSelection.SetActive(false);
        Refresh();
    }
}
