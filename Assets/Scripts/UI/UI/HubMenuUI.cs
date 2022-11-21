using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HubMenuUI : MonoBehaviour
{
    Animator anim;
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        //Debug.Log(anim);
        //this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    void OnMouseOver()
    {
        anim.SetBool("Hover", true);
        //Debug.Log("Hover is true");
    }

    //
    //DOES NOT WORK ON BUTTONS
    //WILL PLAY SOUND WHEN HOVER OVER DIEGETIC UI ELEMENTS 
    //EX: ARMORY, GARAGE, JOB BOARD
    void OnMouseEnter()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    void OnMouseDown()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/ClickDiegetic");
        OpenCanvas();
    }

    //
    //DOES NOT WORK ON SPRITE WITH COLLISIONS
    //WILL PLAY SOUND WHEN HOVER OVER BUTTONS
    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    void OnMouseExit()
    {
        anim.SetBool("Hover", false);
        //Debug.Log("Hover is false");
    }

    // Update is called once per frame
    public void RedirectToJobBoard()
    {
        SceneManager.LoadScene("Test Scene");
    }

    public void OpenCanvas()
    {
        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            g.GetComponent<PolygonCollider2D>().enabled = false;
        }

        List<string> list = new List<string> { "GarageUI", "ArmoryUI", "JobBoardUI"};

        if(!list.Contains(UI.name))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        } 
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/ClickDiegetic");
        }

        //GameObject game = game.GetComponent<GarageConvoyInfo>().
        
        UI.SetActive(true);
    }

    public void CloseCanvas()
    {
        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            //g.GetComponent<HubMenuUI>().enabled = true;
            g.GetComponent<PolygonCollider2D>().enabled = true;
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        UI.SetActive(false);
    }
}
