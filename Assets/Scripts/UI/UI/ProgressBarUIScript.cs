using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUIScript : MonoBehaviour
{
    public Image progressBar;

    [SerializeField]
    BoxCollider2D finishTrigger;

    float currentDist;
    float maxDist;
    float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        lerpSpeed = 6f * Time.deltaTime;

        currentDist = GameManager.Instance.gameEscortee.ActiveEscortee.transform.position.x;
        maxDist = finishTrigger.size.x - currentDist - 2f;
        //maxDist = 40f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current Distance: " + currentDist + "\nFinish line X position: " + maxDist);

        currentDist = GameManager.Instance.gameEscortee.ActiveEscortee.transform.position.x;
        progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, currentDist/maxDist, lerpSpeed);
    }
}
