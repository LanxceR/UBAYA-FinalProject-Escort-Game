using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DebugDisplayReward : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI baseRewardText;
    [SerializeField]
    private TextMeshProUGUI bonusRewardText;
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
        baseRewardText.text = $"{lvl.baseReward}";

        // Update bonus reward text
        string textToDisplay = "";
        if ((lvl.finalReward - lvl.baseReward) >= 0)
            textToDisplay += "+";
        textToDisplay = $"{lvl.finalReward - lvl.baseReward}";
        bonusRewardText.text = textToDisplay;
    }
}
