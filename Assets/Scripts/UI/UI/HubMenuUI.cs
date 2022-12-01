using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubMenuUI : MonoBehaviour
{
    Animator anim;
    [Header("")]
    public GameObject jobBoardUI;
    public GameObject armoryUI;
    public GameObject garageUI;
    public GameObject settingsUI;

    //public GameObject dayBox;
    public TextMeshProUGUI days;
    public TextMeshProUGUI missionCompleted;
    public TextMeshProUGUI missionFailed;


    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();

        /*Transform dayText = dayBox.transform.Find("Day");
        dayText.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.LoadedGameData.daysPassed.ToString();*/

        if(days != null)
        {
            days.text = GameManager.Instance.LoadedGameData.daysPassed.ToString();
        }

        if(missionCompleted != null)
        {
            missionCompleted.text = GameManager.Instance.LoadedGameData.missionsCompleted.ToString();
        }

        if(missionFailed != null)
        {
            missionFailed.text = GameManager.Instance.LoadedGameData.missionsFailed.ToString();
        }


        /*if (GameManager.Instance.LoadedGameData.difficulty == Difficulty.CASUAL)
        {
            if(dayBox != null)
            {
                dayBox.SetActive(false);
            }
        }
        else
        {
            if (dayBox != null)
            {
                dayBox.SetActive(true);
                Transform dayText = dayBox.transform.Find("Day");
                dayText.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.LoadedGameData.daysPassed.ToString();
            }
        }*/

        GameManager.Instance.gameMission.GenerateMissions(GameManager.Instance.LoadedGameData.daysPassed);

        GameManager.Instance.gameData.SaveGame();
    }

    void OnMouseOver()
    {
        anim.SetBool("Hover", true);
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    public void OpenDiegeticCanvas(int i)
    {
        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            g.GetComponent<PolygonCollider2D>().enabled = false;
        }

        if(i == 0)
        {
            jobBoardUI.SetActive(true);
        }
        else if(i == 1)
        {
            armoryUI.SetActive(true);
        }
        else if(i == 2)
        {
            garageUI.SetActive(true);
        }
    }

    public void OpenSettings()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            g.GetComponent<PolygonCollider2D>().enabled = false;
        }

        settingsUI.SetActive(true);
    }

    public void ExitApplication()
    {
        //AUTOSAVE BEFORE EXITING
        GameManager.Instance.gameData.SaveGame();
        Application.Quit();
    }
}
