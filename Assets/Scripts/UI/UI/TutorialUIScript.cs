using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TutorialUIScript : MonoBehaviour
{
    [SerializeField]
    Sprite[] tutorialImages;

    [SerializeField]
    internal Canvas canvas;

    [SerializeField]
    string[] tutorialSubs;

    public Image tutorialImageBox;
    public TextMeshProUGUI tutorialNumber;
    public TextMeshProUGUI tutorialSubsTextBox;

    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;

    public GameObject buttonLeft;

    int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("IntroductionSave" + GameManager.Instance.LoadedGameData.index))
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            PlayerPrefs.SetInt("IntroductionSave" + GameManager.Instance.LoadedGameData.index, 0);

            currentIndex = 0;
            Refresh();

            GameObject[] diegeticObjs;
            diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
            foreach (GameObject g in diegeticObjs)
            {
                g.GetComponent<PolygonCollider2D>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void CycleLeft()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        currentIndex -= 1;
        if (currentIndex < 0)
        {
            currentIndex = tutorialImages.Length - 1;
        }
        Refresh();
    }

    public void CycleRight()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");

        if(currentIndex == tutorialImages.Length -1)
        {
            CloseTutorial();
        }
        else
        {
            currentIndex += 1;
            if (currentIndex > tutorialImages.Length - 1)
            {
                currentIndex = 0;
            }
            Refresh();
        }
    }

    void Refresh()
    {
        tutorialImageBox.sprite = tutorialImages[currentIndex];
        tutorialNumber.text = currentIndex + 1 + "/" + tutorialImages.Length;
        tutorialSubsTextBox.text = tutorialSubs[currentIndex].Replace("\\n", "\n");

        switch (currentIndex)
        {
            case 0:
                buttonLeft.SetActive(false);
                leftText.transform.gameObject.SetActive(false);
                rightText.text = "Next";
                break;
            case 8:
                rightText.text = "END";
                break;
            default:
                buttonLeft.SetActive(true);
                leftText.transform.gameObject.SetActive(true);
                rightText.text = "Next";
                break;
        }
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    void CloseTutorial()
    {
        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            g.GetComponent<PolygonCollider2D>().enabled = true;
        }
        this.gameObject.SetActive(false);
    }
}
