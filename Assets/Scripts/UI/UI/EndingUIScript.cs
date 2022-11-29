using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingUIScript : MonoBehaviour
{
    public TextMeshProUGUI sub;
    private string textContainer;
    void Start()
    {
        textContainer = sub.text + "\nYou have lasted " + GameManager.Instance.LoadedGameData.daysPassed + " days.";
        sub.text = "";

        StartCoroutine(Typewriter(textContainer, sub));
    }

    public void GoBackToMenu()
    {
        GameManager.Instance.gameScene.GotoScene(SceneName.MAIN_HUB);
    }
    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    IEnumerator Typewriter(string text, TextMeshProUGUI label)
    {
        var waitTimer = new WaitForSeconds(.05f);
        foreach (char c in text)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Typewriter");
            label.text = label.text + c;

            if (c == '.')
            {
                yield return new WaitForSeconds(0.8f);
            }
            else if (c == ',')
            {
                yield return new WaitForSeconds(0.2f);
            }

            yield return waitTimer;
        }
    }
}
