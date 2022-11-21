using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobBoardHelperScript : MonoBehaviour
{
    Animator anim;
    public int missionIndex;
    public GameObject parentJobBoardUI;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void OnMouseOver()
    {
        anim.SetBool("Hover", true);
    }

    void OnMouseEnter()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/HoverPaper");
    }

    void OnMouseExit()
    {
        anim.SetBool("Hover", false);
    }

    void OnMouseDown()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/ClickPaper");
        parentJobBoardUI.GetComponent<JobBoardUIScript>().OpenBriefing(missionIndex);
    }
}
