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

    [Header("UI Images")]
    public Sprite job1Image;
    public Sprite job2Image;
    public Sprite job3Image;
    public Sprite jobFinalImage;

    // Start is called before the first frame update

    //This method will check for all available missions.
    //Mission objects will be hidden depending on the status of the game.
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void debugIndex(int i)
    {
        missionIndex = i;

        Debug.Log("Mission number: " + (i + (int)1));
        briefingUI.SetActive(true);

        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            g.GetComponent<PolygonCollider2D>().enabled = false;
        }

        Refresh(missionIndex);
    }

    //This method will load the initial data to the briefingUI texts in accordance with generated mission
    void Refresh(int i)
    {
        //CHECK MISSION LOCATION
        switch (i)
        {
            case 0:
                missionImage.sprite = job1Image;
                break;
            case 1:
                missionImage.sprite = job2Image;
                break;
            case 2:
                missionImage.sprite = job3Image;
                break;
            case 3:
                missionImage.sprite = jobFinalImage;
                break;
        }

        missionReward.text = "$" + GameManager.Instance.MissionDatas[missionIndex].baseReward;
        missionEscorteeChoice.text = GameManager.Instance.LoadedGameData.equippedVehicle.ToString().Replace('_', ' ');
        missionWeaponChoice.text = GameManager.Instance.LoadedGameData.equippedRangedWeapon1.ToString().Replace('_', ' ') + "/"
            + GameManager.Instance.LoadedGameData.equippedRangedWeapon2.ToString().Replace('_', ' ') + "/"
            + GameManager.Instance.LoadedGameData.equippedMeleeWeapon.ToString().Replace('_', ' ');
    }

    public void CloseBriefing()
    {
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
        briefingUI.SetActive(false);
    }
}
