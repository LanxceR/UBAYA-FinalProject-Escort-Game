using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobBoardUIScript : MonoBehaviour
{
    public GameObject jobConfirmationUI;

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
        Debug.Log("Mission number: " + (i + (int)1));
        jobConfirmationUI.SetActive(true);
    }

    //This method will refresh the jobConfiramtionUI text in accordance with generated mission
    void Refresh()
    {

    }
}
