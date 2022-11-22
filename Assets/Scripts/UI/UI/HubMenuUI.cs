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

    public TextMeshProUGUI dayText;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        dayText.text = GameManager.Instance.LoadedGameData.daysPassed.ToString();
        GameManager.Instance.gameMission.GenerateMissions(GameManager.Instance.LoadedGameData.daysPassed);
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
        //needs code
        //idk what code
        //pls help
        //im going insane
        //i want to die
        //UI coding is literal hell
        //why am i struggling with this
        //why do i do this to myself
    }
}
