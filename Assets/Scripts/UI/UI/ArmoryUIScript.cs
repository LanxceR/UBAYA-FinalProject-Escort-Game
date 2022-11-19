using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class ArmoryUIScript : MonoBehaviour
{
    WeaponScript[] weaponList;
    private int currentIndex;
    public GameObject purchasePanel;
    public GameObject ammoPurchasePanel;

    //GAMEOBJECTS
    #region Attached GameObjects

    //General Info
    public GameObject nameObject;
    public GameObject priceButtonObject;
    public GameObject imageObject;

    public GameObject parameterOneObject;
    public GameObject parameterTwoObject;

    public GameObject equipButtonObject;

    public GameObject ammoPanel;
    public GameObject ammoCount;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        weaponList = GameManager.Instance.gameWeapon.weaponPrefabs;

        for (int i = 0; i < weaponList.Length; i++)
        {
            if (weaponList[i].isEquipped == true)
            {
                Transform childPrice = priceButtonObject.transform.Find("Price");
                Transform childPurchased = priceButtonObject.transform.Find("Text");
                Transform childImage = weaponList[i].transform.Find("Weapon Model");
                /*
                Transform parOneSlider = parameterOneObject.transform.Find("Slider");
                Transform parOneText = parameterOneObject.transform.Find("Text");

                Transform parTwoSlider = parameterTwoObject.transform.Find("Slider");
                Transform parTwoText = parameterTwoObject.transform.Find("Text");*/


                nameObject.GetComponent<TextMeshProUGUI>().text = weaponList[i].id.ToString().Replace('_',' ');

                childPrice.GetComponent<TextMeshProUGUI>().text = "Owned";
                childPrice.GetComponentInParent<Button>().interactable = false;
                childPurchased.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

                imageObject.GetComponent<Image>().sprite = childImage.GetComponent<SpriteRenderer>().sprite;

                RefreshSliders(i);
                /*
                //MELEE
                if(weaponList[i].ammoType == AmmoType.NONE)
                {
                    //CHANGE MAX VALUE OF SLIDERS
                    parOneSlider.GetComponent<Slider>().maxValue = (float)20;
                    parTwoSlider.GetComponent<Slider>().maxValue = (float)1.8;

                    //CHANGE NAME OF SLIDERS
                    parOneText.GetComponent<TextMeshProUGUI>().text = "DMG";
                    parTwoText.GetComponent<TextMeshProUGUI>().text = "Speed";

                    //CHANGE VALUES
                    parOneSlider.GetComponent<Slider>().value = weaponList[i].GetComponent<WeaponMeleeAttackScript>().damage;
                    parTwoSlider.GetComponent<Slider>().value = (float)1.8 - weaponList[i].GetComponent<WeaponMeleeAttackScript>().attackDelay;

                    //AMMO PANEL LOCK
                    ammoPanel.SetActive(true);
                }
                //RANGED
                else 
                {
                    //CHANGE MAX VALUE OF SLIDERS
                    parOneSlider.GetComponent<Slider>().maxValue = (float)15;
                    parTwoSlider.GetComponent<Slider>().maxValue = (float)30;

                    //CHANGE NAME OF SLIDERS
                    if (weaponList[i].id.ToString() == "SHOTGUN")
                    {
                        parOneText.GetComponent<TextMeshProUGUI>().text = "DMG (x5)";
                    }
                    else
                    {
                        parOneText.GetComponent<TextMeshProUGUI>().text = "DMG";
                    }

                    parTwoText.GetComponent<TextMeshProUGUI>().text = "Magazine";

                    //CHANGE VALUES
                    parOneSlider.GetComponent<Slider>().value = weaponList[i].GetComponent<WeaponRangedAttackScript>().damage;
                    parTwoSlider.GetComponent<Slider>().value = weaponList[i].ammoMagSize;

                    //AMMO PANEL UNLOCK
                    ammoPanel.SetActive(false);
                }*/

                equipButtonObject.GetComponent<Button>().interactable = false;
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "IN USE";
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

                currentIndex = i;

                break;
            }
        }
    }

    // Update is called once per frame
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
            currentIndex = weaponList.Length - 1;
        }

        Refresh();
    }

    public void CycleRight()
    {
        currentIndex++;

        //Make sure that array index is not below 0
        if (currentIndex > weaponList.Length - 1)
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
        Transform childImage = weaponList[currentIndex].transform.Find("Weapon Model");
        /*
        Transform parOneSlider = parameterOneObject.transform.Find("Slider");
        Transform parOneText = parameterOneObject.transform.Find("Text");

        Transform parTwoSlider = parameterTwoObject.transform.Find("Slider");
        Transform parTwoText = parameterTwoObject.transform.Find("Text");*/

        nameObject.GetComponent<TextMeshProUGUI>().text = weaponList[currentIndex].id.ToString().Replace('_', ' ');

        imageObject.GetComponent<Image>().sprite = childImage.GetComponent<SpriteRenderer>().sprite;

        RefreshSliders();

        /*if (weaponList[currentIndex].ammoType == AmmoType.NONE)
        {
            //CHANGE MAX VALUE OF SLIDERS
            parOneSlider.GetComponent<Slider>().maxValue = (float)20;
            parTwoSlider.GetComponent<Slider>().maxValue = (float)1.8;

            //CHANGE NAME OF SLIDERS
            parOneText.GetComponent<TextMeshProUGUI>().text = "DMG";
            parTwoText.GetComponent<TextMeshProUGUI>().text = "Speed";

            //CHANGE VALUES
            parOneSlider.GetComponent<Slider>().value = weaponList[currentIndex].GetComponent<WeaponMeleeAttackScript>().damage;
            parTwoSlider.GetComponent<Slider>().value = (float)1.8 - weaponList[currentIndex].GetComponent<WeaponMeleeAttackScript>().attackDelay;

            ammoPanel.SetActive(true);
        }
        //RANGED
        else
        {
            //CHANGE MAX VALUE OF SLIDERS
            parOneSlider.GetComponent<Slider>().maxValue = (float)15;
            parTwoSlider.GetComponent<Slider>().maxValue = (float)30;

            //CHANGE NAME OF SLIDERS
            if (weaponList[currentIndex].id.ToString() == "SHOTGUN")
            {
                parOneText.GetComponent<TextMeshProUGUI>().text = "DMG (x5)";
            }
            else
            {
                parOneText.GetComponent<TextMeshProUGUI>().text = "DMG";
            }
            parTwoText.GetComponent<TextMeshProUGUI>().text = "Magazine";

            //CHANGE VALUES
            parOneSlider.GetComponent<Slider>().value = weaponList[currentIndex].GetComponent<WeaponRangedAttackScript>().damage;
            parTwoSlider.GetComponent<Slider>().value = weaponList[currentIndex].ammoMagSize;

            ammoPanel.SetActive(false);
        }*/

        if (weaponList[currentIndex].isOwned == true)
        {
            childPrice.GetComponent<TextMeshProUGUI>().text = "Owned";
            childPrice.GetComponentInParent<Button>().interactable = false;
            childPurchased.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

            if (weaponList[currentIndex].isEquipped == true)
            {
                equipButtonObject.GetComponent<Button>().interactable = false;
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "IN USE";
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
            }
            else
            {
                equipButtonObject.GetComponent<Button>().interactable = true;
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "SELECT AS MAIN WEAPON";
                equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            }
        }
        else
        {
            childPrice.GetComponent<TextMeshProUGUI>().text = "$" + weaponList[currentIndex].price.ToString();
            childPrice.GetComponentInParent<Button>().interactable = true;
            childPurchased.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);

            equipButtonObject.GetComponent<Button>().interactable = false;
            equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().text = "NOT BOUGHT";
            equipButtonObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
        }
    }

    void RefreshSliders(int i = -1)
    {
        if(i == -1)
        {
            i = currentIndex;
        }

        Transform parOneSlider = parameterOneObject.transform.Find("Slider");
        Transform parOneText = parameterOneObject.transform.Find("Text");

        Transform parTwoSlider = parameterTwoObject.transform.Find("Slider");
        Transform parTwoText = parameterTwoObject.transform.Find("Text");

        if (weaponList[currentIndex].ammoType == AmmoType.NONE)
        {
            //CHANGE MAX VALUE OF SLIDERS
            parOneSlider.GetComponent<Slider>().maxValue = (float)20;
            parTwoSlider.GetComponent<Slider>().maxValue = (float)1.8;

            //CHANGE NAME OF SLIDERS
            parOneText.GetComponent<TextMeshProUGUI>().text = "DMG";
            parTwoText.GetComponent<TextMeshProUGUI>().text = "Speed";

            //CHANGE VALUES
            parOneSlider.GetComponent<Slider>().value = weaponList[i].GetComponent<WeaponMeleeAttackScript>().damage;
            parTwoSlider.GetComponent<Slider>().value = (float)1.8 - weaponList[i].GetComponent<WeaponMeleeAttackScript>().attackDelay;

            ammoPanel.SetActive(true);
        }
        //RANGED
        else
        {
            //CHANGE MAX VALUE OF SLIDERS
            parOneSlider.GetComponent<Slider>().maxValue = (float)15;
            parTwoSlider.GetComponent<Slider>().maxValue = (float)30;

            //CHANGE NAME OF SLIDERS
            if (weaponList[currentIndex].id.ToString() == "SHOTGUN")
            {
                parOneText.GetComponent<TextMeshProUGUI>().text = "DMG (x5)";
            }
            else
            {
                parOneText.GetComponent<TextMeshProUGUI>().text = "DMG";
            }
            parTwoText.GetComponent<TextMeshProUGUI>().text = "Magazine";

            //CHANGE VALUES
            parOneSlider.GetComponent<Slider>().value = weaponList[i].GetComponent<WeaponRangedAttackScript>().damage;
            parTwoSlider.GetComponent<Slider>().value = weaponList[i].ammoMagSize;

            ammoPanel.SetActive(false);
        }
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }
}
