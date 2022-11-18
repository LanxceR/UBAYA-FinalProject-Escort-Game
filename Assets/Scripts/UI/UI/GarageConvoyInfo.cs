using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        public GameObject priceObject;
        public GameObject imageObject;
        public GameObject healthSliderObject;
        public GameObject maxSpeedSliderObject;

    #endregion

    void Start()
    {
        //Seek through convoy list to find which convoy is currently being equipped
        for (int i = 0; i < convoys.Length; i++)
        {
            if (convoys[i].isEquipped == true)
            {
                //update convoy name
                nameObject.GetComponent<TextMeshProUGUI>().text = convoys[i].convoyName;

                //update convoy price
                if (convoys[i].isPurchased == true)
                {
                    priceObject.GetComponent<TextMeshProUGUI>().text = "Owned";
                    priceObject.GetComponentInParent<Button>().interactable = false;
                }
                else
                {
                    priceObject.GetComponent<TextMeshProUGUI>().text = "$" + convoys[i].convoyPrice.ToString();
                    priceObject.GetComponentInParent<Button>().interactable = true;
                }

                //update convoy image
                imageObject.GetComponent<Image>().sprite = convoys[i].convoyImage;

                //update convoy sliders
                //hp
                healthSliderObject.GetComponent<Slider>().value = convoys[i].healthValue;
                //maxspd
                maxSpeedSliderObject.GetComponent<Slider>().value = convoys[i].maxSpeedValue;

                currentIndex = i;

                break;
            }

            //USE BUS AS MAIN IF SOMETHING BREAKS
            #region code
            nameObject.GetComponent<TextMeshProUGUI>().text = convoys[0].convoyName;

            if (convoys[0].isPurchased == true)
            {
                priceObject.GetComponent<TextMeshProUGUI>().text = "Owned";
                priceObject.GetComponentInParent<Button>().interactable = false;
            }
            else
            {
                priceObject.GetComponent<TextMeshProUGUI>().text = "$" + convoys[0].convoyPrice.ToString();
                priceObject.GetComponentInParent<Button>().interactable = true;
            }

            imageObject.GetComponent<Image>().sprite = convoys[0].convoyImage;
            healthSliderObject.GetComponent<Slider>().value = convoys[0].healthValue;
            maxSpeedSliderObject.GetComponent<Slider>().value = convoys[0].maxSpeedValue;

            currentIndex = 0;
            #endregion
        }
    }

    public void CycleLeft()
    {
        Debug.Log("Current Index: " + currentIndex);
        currentIndex--;
        Debug.Log("Current Index: " + currentIndex);

        //Make sure that array index is not below 0
        if (currentIndex < 0)
        {
            currentIndex = 2;
        }

        #region code
        nameObject.GetComponent<TextMeshProUGUI>().text = convoys[currentIndex].convoyName;

        if (convoys[currentIndex].isPurchased == true)
        {
            priceObject.GetComponent<TextMeshProUGUI>().text = "Owned";
            priceObject.GetComponentInParent<Button>().interactable = false;
        }
        else
        {
            priceObject.GetComponent<TextMeshProUGUI>().text = "$" + convoys[currentIndex].convoyPrice.ToString();
            priceObject.GetComponentInParent<Button>().interactable = true;
        }

        imageObject.GetComponent<Image>().sprite = convoys[currentIndex].convoyImage;
        healthSliderObject.GetComponent<Slider>().value = convoys[currentIndex].healthValue;
        maxSpeedSliderObject.GetComponent<Slider>().value = convoys[currentIndex].maxSpeedValue;
        #endregion
    }

    public void CycleRight()
    {
        currentIndex++;

        //Make sure that array index is not below 0
        if (currentIndex > 2)
        {
            currentIndex = 0;
        }

        #region code
        nameObject.GetComponent<TextMeshProUGUI>().text = convoys[currentIndex].convoyName;

        if (convoys[currentIndex].isPurchased == true)
        {
            priceObject.GetComponent<TextMeshProUGUI>().text = "Owned";
            priceObject.GetComponentInParent<Button>().interactable = false;
        }
        else
        {
            priceObject.GetComponent<TextMeshProUGUI>().text = "$" + convoys[currentIndex].convoyPrice.ToString();
            priceObject.GetComponentInParent<Button>().interactable = true;
        }

        imageObject.GetComponent<Image>().sprite = convoys[currentIndex].convoyImage;
        healthSliderObject.GetComponent<Slider>().value = convoys[currentIndex].healthValue;
        maxSpeedSliderObject.GetComponent<Slider>().value = convoys[currentIndex].maxSpeedValue;
        #endregion
    }

    void Update()
    {
        
    }
}
