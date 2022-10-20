using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HubMenuUI : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        Debug.Log(anim);
        //this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    void OnMouseOver()
    {
        anim.SetBool("Hover", true);
        Debug.Log("Hover is true");
    }

    void OnMouseExit()
    {
        anim.SetBool("Hover", false);
        Debug.Log("Hover is false");
    }

    // Update is called once per frame
    public void RedirectToJobBoard()
    {
        SceneManager.LoadScene("Test Scene");
    }
}
