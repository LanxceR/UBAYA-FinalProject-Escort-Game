using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class Convoy
{
    public string convoyName;
    public float convoyPrice;
    public Sprite convoyImage;
    public float healthValue;
    public float maxSpeedValue;
    public bool isEquipped;
    public bool isPurchased;
}

public class GarageConvoyInfo : MonoBehaviour
{
    public Convoy[] convoys;
    private int currentIndex;

    #region Attached GameObjects

        public GameObject nameObject;
        public GameObject priceButtonObject;
        public GameObject imageObject;
        public GameObject healthSliderObject;
        public GameObject maxSpeedSliderObject;
        public GameObject equipButtonObject;

    #endregion

    void Start()
    {
        //Seek through convoy list to find which convoy is currently being equipped
        for (int i = 0; i < convoys.Length; i++)
        {
            Transform childPrice = priceButtonObject.transform.Find("Price");
            Transform childPurchased = priceButtonObject.transform.Find("Text");

            if (convoys[i].isEquipped == true)
            {
                //update convoy name
                nameObject.GetComponent<TextMeshProUGUI>().text = convoys[i].convoyName;

                //update convoy price and button
                childPrice.GetComponent<TextMeshProUGUI>().text = "Owned";
                childPrice.GetComponentInParent<Button>().interactable = false;

                childPurchased.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

                //update convoy image
                imageObject.GetComponent<Image>().sprite = convoys[i].convoyImage;

                //update convoy sliders
                //hp
                healthSliderObject.GetComponent<Slider>().value = convoys[i].healthValue;
                //maxspd
                maxSpeedSliderObject.GetComponent<Slider>().value = convoys[i].maxSpeedValue;

                //isEquipped button disable
                equipButtonObject.GetComponent<Button>().interactable = false;
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "IN USE";
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

                currentIndex = i;

                break;
            }

            //USE BUS AS MAIN IF SOMETHING BREAKS
            #region code
            nameObject.GetComponent<TextMeshProUGUI>().text = convoys[0].convoyName;

            childPrice.GetComponent<TextMeshProUGUI>().text = "Owned";
            childPrice.GetComponentInParent<Button>().interactable = false;
            childPurchased.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

            imageObject.GetComponent<Image>().sprite = convoys[0].convoyImage;
            healthSliderObject.GetComponent<Slider>().value = convoys[0].healthValue;
            maxSpeedSliderObject.GetComponent<Slider>().value = convoys[0].maxSpeedValue;
            equipButtonObject.GetComponent<Button>().interactable = false;
            equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "IN USE";
            equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

            currentIndex = 0;
            #endregion
        }
    }

    public void CycleLeft()
    {
        currentIndex--;

        //Make sure that array index is not below 0
        if (currentIndex < 0)
        {
            currentIndex = 2;
        }

        Refresh(currentIndex);
    }

    public void CycleRight()
    {
        currentIndex++;

        //Make sure that array index is not below 0
        if (currentIndex > 2)
        {
            currentIndex = 0;
        }

        Refresh(currentIndex);
    }

    private void Refresh(int i)
    {
        #region code
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");

        //FIND CHILD OF THE PURCHASE BUTTONS (purchase and price)
        Transform childPrice = priceButtonObject.transform.Find("Price");
        Transform childPurchased = priceButtonObject.transform.Find("Text");

        nameObject.GetComponent<TextMeshProUGUI>().text = convoys[i].convoyName;

        if (convoys[i].isPurchased == true)
        {
            childPrice.GetComponent<TextMeshProUGUI>().text = "Owned";
            childPrice.GetComponentInParent<Button>().interactable = false;
            childPurchased.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

            if (convoys[i].isEquipped == true)
            {
                equipButtonObject.GetComponent<Button>().interactable = false;
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "IN USE";
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
            }
            else
            {
                equipButtonObject.GetComponent<Button>().interactable = true;
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "SELECT AS MAIN CONVOY";
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            }
        }
        else
        {
            childPrice.GetComponent<TextMeshProUGUI>().text = "$" + convoys[i].convoyPrice.ToString();
            childPrice.GetComponentInParent<Button>().interactable = true;
            childPurchased.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);

            //LOCK AWAY THE BUTTON TO EQUIP
            equipButtonObject.GetComponent<Button>().interactable = false;
            equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "NOT BOUGHT";
            equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
        }

        imageObject.GetComponent<Image>().sprite = convoys[i].convoyImage;
        healthSliderObject.GetComponent<Slider>().value = convoys[i].healthValue;
        maxSpeedSliderObject.GetComponent<Slider>().value = convoys[i].maxSpeedValue;

        #endregion
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
         {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void SelectAsMain()
    {
        for (int i = 0; i < convoys.Length; i++)
        {
            if(i != currentIndex)
            {
                convoys[i].isEquipped = false;
            }
            else
            {
                convoys[i].isEquipped = true;
            }
        }
        Refresh(currentIndex);
    }

}
