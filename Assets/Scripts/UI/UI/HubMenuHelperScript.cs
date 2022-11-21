using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMenuHelperScript : MonoBehaviour
{
    Animator anim;
    public int canvasIndex;
    public GameObject parentHubUI;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void OnMouseOver()
    {
        anim.SetBool("Hover", true);
    }
    void OnMouseExit()
    {
        anim.SetBool("Hover", false);
    }

    void OnMouseEnter()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    void OnMouseDown()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/ClickDiegetic");
        parentHubUI.GetComponent<HubMenuUI>().OpenDiegeticCanvas(canvasIndex);
    }
}
