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
                Transform childPrice = priceButtonObject.transform.Find("Price");
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
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

                currentIndex = i;

                break;
            }
        }
    }

    //BUTTON COLOR
    void Update()
    {
        //AFTER PRESSING BUTTONS IN THE UI, COLOR FADES BACK TO NORMAL

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

        Refresh();
    }
    #endregion

    //REFRESH
    private void Refresh()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");

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

               //if (escorteeList[currentIndex].isEquipped == true)
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
    }

    public void SelectAsMainEscortee()
    {
        GameManager.Instance.LoadedGameData.equippedVehicle = escorteeList[currentIndex].id;
        /*for (int i = 0; i < escorteeList.Length; i++)
        {
            if (i != currentIndex)
            {
                escorteeList[i].isEquipped = false;
            }
            else
            {
                escorteeList[i].isEquipped = true;
            }
        }*/
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
        foreach (GameObject script in diegeticObjs)
        {
            script.GetComponent<HubMenuUI>().enabled = true;
            script.GetComponent<PolygonCollider2D>().enabled = true;
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        this.gameObject.SetActive(false);
    }

    #region Purchase Methods
    //OPEN PURCHASE PANEL
    public void OpenPurchasePanel()
    {
        purchasePanel.SetActive(true);
        Transform escorteeName = purchasePanel.transform.Find("PurchasedConvoyName");
        escorteeName.GetComponent<TextMeshProUGUI>().text = escorteeList[currentIndex].id.ToString().Replace('_', ' ');
    }

    //PURCHASE AND SET OWNED
    public void ConfirmPurchase()
    {
        escorteeList[currentIndex].isOwned = true;
        Refresh();
        ClosePurchasePanel();
    }

    //CLOSE PURCHASE PANEL
    public void ClosePurchasePanel()
    {
        purchasePanel.SetActive(false);
    }
    #endregion
}
