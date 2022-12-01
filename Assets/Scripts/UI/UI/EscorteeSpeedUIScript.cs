using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EscorteeSpeedUIScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Image speedBar;
    public TextMeshProUGUI speedText;
    float speedStage;
    float lerpSpeed;

    void Start()
    {
        speedStage = 0;
        GetSpeedStage();
        lerpSpeed = 25f * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        GetSpeedStage();
        switch(speedStage)
        {
            case 0:
                //speedBar.fillAmount = 0f;
                speedBar.fillAmount = Mathf.Lerp(speedBar.fillAmount, 0f, lerpSpeed);
                speedText.text = "IDLE";
                break;
            case 1:
                //speedBar.fillAmount = .33f;
                speedBar.fillAmount = Mathf.Lerp(speedBar.fillAmount, .33f, lerpSpeed);
                speedText.text = "1/4x";
                break;
            case 2:
                //speedBar.fillAmount = .66f;
                speedBar.fillAmount = Mathf.Lerp(speedBar.fillAmount, .66f, lerpSpeed);
                speedText.text = "1/2x";
                break;
            case 3:
                //speedBar.fillAmount = 1f;
                speedBar.fillAmount = Mathf.Lerp(speedBar.fillAmount, 1f, lerpSpeed);
                speedText.text = "MAX";
                break;
        }
    }

    void GetSpeedStage()
    {
        if (GameManager.Instance.gameEscortee.ActiveEscortee != null)
        {
            speedStage = GameManager.Instance.gameEscortee.ActiveEscortee.escorteeMovementScript.speedStage;
        }
    }
}
