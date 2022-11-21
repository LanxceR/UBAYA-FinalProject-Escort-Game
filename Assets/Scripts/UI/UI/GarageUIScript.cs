using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GarageUIScript : MonoBehaviour
{
    //FIRST IN EXECUTION: RETRIEVE THE LIST OF PREFABS (ESCORTEES)
    //CALL THE CONTENTS OF THE LIST WITH FOR, FOREACH, OR WITH INDEX ---> "escorteeList[index];
    //MODIFY CONTENT WITH INBUILT METHODS

    EscorteeScript[] escorteeList;
    private int currentIndex;
    public GameObject purchasePanel;

    //GAMEOBJECTS
    #region Attached GameObjects

        public GameObject nameObject;
        public GameObject priceButtonObject;
        public GameObject imageObject;
        public GameObject healthSliderObject;
        public GameObject maxSpeedSliderObject;
        public GameObject equipButtonObject;

        [Header("Cash Text Box")]
        public TextMeshProUGUI cashOwnedText;

        [Header("Popup")]
        public GameObject popup;

    #endregion


    //INITIALIZE
    void Start()
    {
        escorteeList = GameManager.Instance.gameEscortee.EscorteePrefabs;

        EscorteeID equippedEscortee = GameManager.Instance.LoadedGameData.equippedVehicle;

        if(equippedEscortee is EscorteeID.NONE)
        {
            equippedEscortee = EscorteeID.BUS;
        }

        //FIND CURRENTLY IN USE ESCORTEE

        for (int i =0; i< escorteeList.Length; i++)
        {
            if(escorteeList[i].id == equippedEscortee)
            {
                #region unused code
                /*Transform childPrice = priceButtonObject.transform.Find("Price");
                Transform childPurchased = priceButtonObject.transform.Find("Text");
                Transform childImage = escorteeList[i].transform.Find("Model");

                nameObject.GetComponent<TextMeshProUGUI>().text = escorteeList[i].id.ToString().Replace('_', ' ');

                childPrice.GetComponent<TextMeshProUGUI>().text = "Owned";
                childPrice.GetComponentInParent<Button>().interactable = false;
                childPurchased.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

                imageObject.GetComponent<Image>().sprite = childImage.GetComponent<SpriteRenderer>().sprite;

                healthSliderObject.GetComponent<Slider>().value = escorteeList[i].health;
                maxSpeedSliderObject.GetComponent<Slider>().value = escorteeList[i].maxSpeed;

                equipButtonObject.GetComponent<Button>().interactable = false;
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "IN USE";
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);*/
                #endregion

                popup.SetActive(false);
                currentIndex = i;

                Refresh();

                break;
            }
        }
    }

    //BUTTON COLOR
    void Update()
    {
        //AFTER PRESSING BUTTONS IN THE UI, COLOR FADES BACK TO NORMAL
        cashOwnedText.text = "$" + GameManager.Instance.LoadedGameData.money.ToString();

        if (Input.GetMouseButtonUp(0))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    //CYCLING
    #region Cycle Methods
    public void CycleLeft()
    {
        currentIndex--;

        //Make sure that array index is not below 0
        if (currentIndex < 0)
        {
            currentIndex = 2;
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        Refresh();
    }

    public void CycleRight()
    {
        currentIndex++;

        //Make sure that array index is not below 0
        if (currentIndex > 2)
        {
            currentIndex = 0;
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        Refresh();
    }
    #endregion

    //REFRESH
    private void Refresh()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        cashOwnedText.text = "$" + GameManager.Instance.LoadedGameData.money.ToString();
        popup.SetActive(false);

        Transform childPrice = priceButtonObject.transform.Find("Price");
        Transform childPurchased = priceButtonObject.transform.Find("Text");
        Transform childImage = escorteeList[currentIndex].transform.Find("Model");

        nameObject.GetComponent<TextMeshProUGUI>().text = escorteeList[currentIndex].id.ToString().Replace('_',' ');

        imageObject.GetComponent<Image>().sprite = childImage.GetComponent<SpriteRenderer>().sprite;
        healthSliderObject.GetComponent<Slider>().value = escorteeList[currentIndex].health;
        maxSpeedSliderObject.GetComponent<Slider>().value = escorteeList[currentIndex].maxSpeed;

        if (escorteeList[currentIndex].isOwned == true)
        {
            childPrice.GetComponent<TextMeshProUGUI>().text = "Owned";
            childPrice.GetComponentInParent<Button>().interactable = false;
            childPurchased.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

            if (escorteeList[currentIndex].id == GameManager.Instance.LoadedGameData.equippedVehicle)
            {
                equipButtonObject.GetComponent<Button>().interactable = false;
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "IN USE";
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

                Debug.Log("Name: " + escorteeList[currentIndex].name +
                    "Owned: " + escorteeList[currentIndex].isOwned +
                    "Equipped: " + escorteeList[currentIndex].isEquipped);
            }
            else
            {
                equipButtonObject.GetComponent<Button>().interactable = true;
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "SELECT AS MAIN CONVOY";
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);

                Debug.Log("Name: " + escorteeList[currentIndex].name +
                    "Owned: " + escorteeList[currentIndex].isOwned +
                    "Equipped: " + escorteeList[currentIndex].isEquipped);
            }
        }
        else
        {
            childPrice.GetComponent<TextMeshProUGUI>().text = "$" + escorteeList[currentIndex].price.ToString();
            childPrice.GetComponentInParent<Button>().interactable = true;
            childPurchased.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);

            equipButtonObject.GetComponent<Button>().interactable = false;
            equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "NOT BOUGHT";
            equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

            Debug.Log("Name: " + escorteeList[currentIndex].name +
                    "Owned: " + escorteeList[currentIndex].isOwned +
                    "Equipped: " + escorteeList[currentIndex].isEquipped);
        }

        GameManager.Instance.gameData.SaveGame();
    }

    public void SelectAsMainEscortee()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        GameManager.Instance.LoadedGameData.equippedVehicle = escorteeList[currentIndex].id;
        Refresh();
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    public void CloseCanvas()
    {
        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            g.GetComponent<PolygonCollider2D>().enabled = true;
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        this.gameObject.SetActive(false);
        popup.SetActive(false);
    }
    /*
    public void CloseCanvasFromBriefing()
    {
        this.gameObject.SetActive(false);
    }*/

    #region Purchase Methods
    //OPEN PURCHASE PANEL
    public void OpenPurchasePanel()
    {
        if(GameManager.Instance.LoadedGameData.money < escorteeList[currentIndex].price)
        {
            StartCoroutine(ShowPopup(.8f));
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
            purchasePanel.SetActive(true);
            Transform escorteeName = purchasePanel.transform.Find("PurchasedConvoyName");
            escorteeName.GetComponent<TextMeshProUGUI>().text = escorteeList[currentIndex].id.ToString().Replace('_', ' ');

            Transform escorteePriceText = purchasePanel.transform.Find("Button_Purchase/Price");
            escorteePriceText.GetComponent<TextMeshProUGUI>().text = "$" + escorteeList[currentIndex].price;
        }
    }

    //PURCHASE AND SET OWNED
    public void ConfirmPurchase()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Purchase");
        escorteeList[currentIndex].isOwned = true;

        GameManager.Instance.LoadedGameData.ownedVehicles.Add(escorteeList[currentIndex].id);

        GameManager.Instance.LoadedGameData.money = (float)GameManager.Instance.LoadedGameData.money - (float)escorteeList[currentIndex].price;
        Refresh();
        ClosePurchasePanel();
    }

    //CLOSE PURCHASE PANEL
    public void ClosePurchasePanel()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        purchasePanel.SetActive(false);
    }

    public void CloseGarageUI()
    {
        GameObject[] diegeticObjs;
        diegeticObjs = GameObject.FindGameObjectsWithTag("Diegetic");
        foreach (GameObject g in diegeticObjs)
        {
            g.GetComponent<PolygonCollider2D>().enabled = true;
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        this.gameObject.SetActive(false);
    }
    #endregion


    IEnumerator ShowPopup(float delay)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/PurchaseFail");
        popup.SetActive(true);
        yield return new WaitForSeconds(delay);
        popup.SetActive(false);
    }
}
