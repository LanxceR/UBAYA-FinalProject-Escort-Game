using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUIScript : MonoBehaviour
{
    public Image progressBar;

    //[SerializeField]
    //BoxCollider2D finishTrigger;

    float currentDist;
    float maxDist;
    float lerpSpeed;

    float playerPos;

    [SerializeField]
    GameObject playerMarker;

    [SerializeField]
    GameObject[] triggerMarkers;
    int triggerIndex;

    [SerializeField]
    GameObject[] blockadeMarkers;
    int blockadeIndex;

    float markerPos;

    // Start is called before the first frame update
    void Start()
    {
        lerpSpeed = 6f * Time.deltaTime;
        currentDist = GameManager.Instance.gameEscortee.ActiveEscortee.transform.position.x;

        triggerIndex = 0;

        //it does what it does.
        //thank you darren for the suggestion
        //no sliders cuz sliders are ass and its a terrible idea
        GetAllTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        //UPDATE PLAYER POS
        playerPos = Mathf.Clamp((((GameManager.Instance.gamePlayer.ActivePlayer.transform.position.x / maxDist) * 400 - 200)/100), -2, 2);
        playerMarker.transform.position = new Vector3(playerPos, this.transform.position.y, this.transform.position.z);

        currentDist = GameManager.Instance.gameEscortee.ActiveEscortee.transform.position.x;
        progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, currentDist/maxDist, lerpSpeed);
    }

    void GetAllTrigger()
    {
        //pretty simple, get all the triggers. 
        //id rather not resort to using tags or find but unity has forced my hand
        //i created a new blockade tag to store all blockades (because it was untagged earlier)

        GameObject[] listOfTriggers = GameObject.FindGameObjectsWithTag("Trigger");
        GameObject[] listOfBlockades = GameObject.FindGameObjectsWithTag("Blockade");

        //make sure that the finish trigger position is retrieved first
        //this is just in case if its returned on the last of the array and breaks my spaghetti code

        foreach(GameObject go in listOfTriggers)
        {
            if (go.name.Contains("Finish"))
            {
                maxDist = go.transform.position.x;
                break;
            }
        }

        //after getting the maxDist (length of the map), we can now perform calculations with it
        //the formula is (objectPosition / maxDist) * 400 - 200) / 100
        //400 = width of UI
        //200 = half width of UI
        //the two numbers above can be substituted by retrieving the width of the bar so it scales automatically without tweaking code, but since this ain't changing, im keeping it this way
        //also im too lazy

        //divide by 100 so the UI marker doesnt fly off to the edge of the solar system
        //the result is then clamped between -2 and 2 as a hard limit
        //ta-da
        //the time is 2:30 in the night and i havent gotten out of this prison yet
        //god save my soul

        //now we retrieve all of the markers with the foreach below
        //enable the gameobjects that are used for markers
        //the rest will be disabled by default

        //in the script, two different arrays exist
        //one stores the markers for triggers, the other stores markers for blockades

        foreach (GameObject go in listOfTriggers)
        {
            if (go.name.Contains("Big"))
            {
                markerPos = Mathf.Clamp((((go.transform.position.x / maxDist) * 400 - 200) / 100), -2, 2);
                triggerMarkers[triggerIndex].transform.position = new Vector3(markerPos, this.transform.position.y, this.transform.position.z);
                triggerMarkers[triggerIndex].SetActive(true);
                triggerIndex++;
            }
        }

        foreach (GameObject go in listOfBlockades)
        {
            markerPos = Mathf.Clamp((((go.transform.position.x / maxDist) * 400 - 200) / 100), -2, 2);
            blockadeMarkers[blockadeIndex].transform.position = new Vector3(markerPos, this.transform.position.y, this.transform.position.z);
            blockadeMarkers[blockadeIndex].SetActive(true);
            blockadeIndex++;
        }
    }
}
