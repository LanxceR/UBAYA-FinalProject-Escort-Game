using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void CloseSettingsUI()
    {
        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            g.GetComponent<PolygonCollider2D>().enabled = true;
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        this.gameObject.SetActive(false);
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }
}
