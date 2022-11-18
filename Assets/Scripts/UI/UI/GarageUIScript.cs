using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    public void CloseCanvas()
    {
        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject script in diegeticObjs)
        {
            script.GetComponent<HubMenuUI>().enabled = true;
            script.GetComponent<PolygonCollider2D>().enabled = true;
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        this.gameObject.SetActive(false);
    }
}
