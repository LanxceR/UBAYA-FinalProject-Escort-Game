using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplayTimer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI timerTextFormatted;
    [SerializeField]
    private LevelManager lvl;

    // Start is called before the first frame update
    void Start()
    {
        lvl = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        string textToDisplay = $"{lvl.timePassed}";

        // Update ammo counter text
        timerText.text = textToDisplay;

        float minutes = Mathf.FloorToInt(lvl.timePassed / 60);
        float seconds = Mathf.FloorToInt(lvl.timePassed % 60);
        float milliSeconds = (lvl.timePassed % 1) * 1000;
        timerTextFormatted.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
    }
}
