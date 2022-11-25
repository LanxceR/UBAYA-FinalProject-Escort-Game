using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneTypewriterScript : MonoBehaviour
{
    public TextMeshProUGUI sub;
    private string subContainer;

    // Start is called before the first frame update
    void Start()
    {
        subContainer = sub.text;
        sub.text = "";
    }

    public void Type()
    {
        StartCoroutine(Typewriter(subContainer, sub));
    }

    IEnumerator Typewriter(string text, TextMeshProUGUI label)
    {
        var waitTimer = new WaitForSeconds(.05f);
        foreach (char c in text)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Typewriter");
            label.text = label.text + c;

            if(c == '.')
            {
                yield return new WaitForSeconds(0.8f);
            }
            else if(c == ',')
            {
                yield return new WaitForSeconds(0.2f);
            }

            yield return waitTimer;
        }
    }

}
