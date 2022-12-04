using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameMissionSuccessDisplayInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI baseRewardText;
    [SerializeField]
    private TextMeshProUGUI bonusRewardText;
    [SerializeField]
    private TextMeshProUGUI moneyText;

    public void DisplayMissionEndDetail(float reward)
    {
        float baseReward = Mathf.Round(GameManager.Instance.LoadedMissionData.baseReward);
        baseRewardText.text = baseReward.ToString();

        float bonusReward = Mathf.Round(reward - GameManager.Instance.LoadedMissionData.baseReward);
        if (bonusReward >= 0) 
            bonusRewardText.color = new Color(28f / 255, 190f / 255, 52f / 255); // Green for positive
        else 
            bonusRewardText.color = new Color(190f / 255, 42f / 255, 28f / 255); // Red for negative
        bonusRewardText.text = bonusReward.ToString();

        float money = Mathf.Round(GameManager.Instance.LoadedGameData.money);
        moneyText.text = money.ToString();

        StartCoroutine(DisplayRewardsCoroutine());
    }

    private IEnumerator DisplayRewardsCoroutine()
    {
        baseRewardText.gameObject.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
        yield return new WaitForSeconds(0.5f);
        bonusRewardText.gameObject.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
        yield return new WaitForSeconds(0.5f);
        moneyText.gameObject.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }
}
