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
    public GameObject imageObject;

    [Header("Buttons")]
    public GameObject buttonTopObject;
    public GameObject buttonBottomObject;
    public GameObject buttonPurchaseAmmo;

    [Header("Stats Texts")]
    public GameObject parameterOneObject;
    public GameObject parameterTwoObject;

    [Header("Ammo Panel")]
    public GameObject ammoPanel;
    public GameObject ammoCount;

    [Header("Cash Text Box")]
    public TextMeshProUGUI cashOwnedText;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        weaponList = GameManager.Instance.gameWeapon.weaponPrefabs;

        //USE EQUIPPED MELEE WEAPON AS DEFAULT
        WeaponID equippedWeapon = GameManager.Instance.LoadedGameData.equippedMeleeWeapon;

        for (int i = 0; i < weaponList.Length; i++)
        {
            if (weaponList[i].id == equippedWeapon)
            {
                Transform childImage = weaponList[i].transform.Find("Weapon Model");

                nameObject.GetComponent<TextMeshProUGUI>().text = weaponList[i].id.ToString().Replace('_',' ');

                imageObject.GetComponent<Image>().sprite = childImage.GetComponent<SpriteRenderer>().sprite;

                Refresh();

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

    void PlayClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
    }


    #region Refresh Methods
    //MAIN REFRESH METHOD
    private void Refresh()
    {
        //REFRESH MONEY
        cashOwnedText.text = "$" + GameManager.Instance.LoadedGameData.money.ToString();

        Transform childImage = weaponList[currentIndex].transform.Find("Weapon Model");

        nameObject.GetComponent<TextMeshProUGUI>().text = weaponList[currentIndex].id.ToString().Replace('_', ' ');

        imageObject.GetComponent<Image>().sprite = childImage.GetComponent<SpriteRenderer>().sprite;

        RefreshSliders(currentIndex);
        RefreshButtons();
        RefreshAmmoPanel();

        #region UNUSED CODE
        /*if (weaponList[currentIndex].isOwned == true)
        {
            childPrice.GetComponent<TextMeshProUGUI>().text = "Owned";
            childPrice.GetComponentInParent<Button>().interactable = false;
            childText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);

            if (weaponList[currentIndex].isEquipped == true)
            {
                buttonBottomObject.GetComponent<Button>().interactable = false;
                buttonBottomObject.GetComponentInChildren<TextMeshProUGUI>().text = "IN USE";
                buttonBottomObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
            }
            else
            {
                buttonBottomObject.GetComponent<Button>().interactable = true;
                buttonBottomObject.GetComponentInChildren<TextMeshProUGUI>().text = "SELECT AS MAIN WEAPON";
                buttonBottomObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            }
        }
        else
        {
            childPrice.GetComponent<TextMeshProUGUI>().text = "$" + weaponList[currentIndex].price.ToString();
            childPrice.GetComponentInParent<Button>().interactable = true;
            childText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);

            buttonBottomObject.GetComponent<Button>().interactable = false;
            buttonBottomObject.GetComponentInChildren<TextMeshProUGUI>().text = "NOT BOUGHT";
            buttonBottomObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
        }*/
        #endregion
    }

    //REFRESHES THE VALUES OF SLIDERS
    void RefreshSliders(int i)
    {
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

    //REFRESHES THE STATE OF THE TWO BUTTONS
    void RefreshButtons()
    {
        WeaponID equippedFirstSlot = GameManager.Instance.LoadedGameData.equippedRangedWeapon1;
        WeaponID equippedSecondSlot = GameManager.Instance.LoadedGameData.equippedRangedWeapon2;
        WeaponID equippedMelee = GameManager.Instance.LoadedGameData.equippedMeleeWeapon;

        Transform childTextTop = buttonTopObject.transform.Find("Text");
        Transform childPrice = buttonTopObject.transform.Find("Price");

        Transform childTextBottom = buttonBottomObject.transform.Find("Text");

        if (weaponList[currentIndex].isOwned)
        {
            //CHECK IF WEAPON IS MELEE OR RANGED
            if (weaponList[currentIndex].ammoType == AmmoType.NONE)
            {
                buttonTopObject.GetComponent<Button>().interactable = false;
                childTextTop.GetComponent<TextMeshProUGUI>().text = "Purchase";
                childTextTop.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
                childPrice.GetComponent<TextMeshProUGUI>().text = "Owned";

                if (equippedMelee == weaponList[currentIndex].id)
                {
                    //DISABLE BOTTOM BUTTON
                    buttonBottomObject.GetComponent<Button>().interactable = false;
                    childTextBottom.GetComponent<TextMeshProUGUI>().text = "Equipped";
                    childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
                }
                else
                {
                    //ENABLE BOTTOM BUTTON
                    buttonBottomObject.GetComponent<Button>().interactable = true;
                    childTextBottom.GetComponent<TextMeshProUGUI>().text = "Equip";
                    childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                }
            }
            else
            {
                buttonTopObject.GetComponent<Button>().interactable = true;
                buttonBottomObject.GetComponent<Button>().interactable = true;

                //FIRST SLOT
                if (equippedFirstSlot == weaponList[currentIndex].id)
                {
                    childTextTop.GetComponent<TextMeshProUGUI>().text = "EQUIPPED / UNEQUIP";
                    childTextTop.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                    childPrice.GetComponent<TextMeshProUGUI>().text = "";

                    childTextBottom.GetComponent<TextMeshProUGUI>().text = "EQUIP TO SLOT 2";
                    childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                }
                //SECOND SLOT
                else if (equippedSecondSlot == weaponList[currentIndex].id)
                {
                    childTextTop.GetComponent<TextMeshProUGUI>().text = "EQUIP TO SLOT 1";
                    childTextTop.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                    childPrice.GetComponent<TextMeshProUGUI>().text = "";

                    childTextBottom.GetComponent<TextMeshProUGUI>().text = "EQUIPPED / UNEQUIP";
                    childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                }
                else
                {
                    childTextTop.GetComponent<TextMeshProUGUI>().text = "EQUIP TO SLOT 1";
                    childTextTop.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                    childPrice.GetComponent<TextMeshProUGUI>().text = "";

                    childTextBottom.GetComponent<TextMeshProUGUI>().text = "EQUIP TO SLOT 2";
                    childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                }
            }

        }
        else
        {
            buttonTopObject.GetComponent<Button>().interactable = true;
            childTextTop.GetComponent<TextMeshProUGUI>().text = "Purchase";
            childTextTop.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
            childPrice.GetComponent<TextMeshProUGUI>().text = "$" + weaponList[currentIndex].price;

            buttonBottomObject.GetComponent<Button>().interactable = false;
            childTextBottom.GetComponent<TextMeshProUGUI>().text = "NOT PURCHASED";
            childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
        }

        #region UNUSED CODE (IM AN IDIOT)
        /*for (int i = 0; i < weaponList.Length; i++)
        {
            if (weaponList[i].isOwned)
            {
                //CHECK IF WEAPON IS MELEE OR RANGED
                if (weaponList[i].ammoType == AmmoType.NONE)
                {
                    buttonTopObject.GetComponent<Button>().interactable = false;
                    childTextTop.GetComponent<TextMeshProUGUI>().text = "Purchase";
                    childTextTop.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
                    childPrice.GetComponent<TextMeshProUGUI>().text = "Owned";

                    if (equippedMelee == weaponList[i].id)
                    {
                        //DISABLE BOTTOM BUTTON
                        buttonBottomObject.GetComponent<Button>().interactable = false;
                        childTextBottom.GetComponent<TextMeshProUGUI>().text = "Equipped";
                        childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
                    }
                    else
                    {
                        //ENABLE BOTTOM BUTTON
                        buttonBottomObject.GetComponent<Button>().interactable = true;
                        childTextBottom.GetComponent<TextMeshProUGUI>().text = "Equip";
                        childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                    }
                }
                else
                {
                    buttonTopObject.GetComponent<Button>().interactable = true;
                    buttonBottomObject.GetComponent<Button>().interactable = true;

                    //FIRST SLOT
                    if (equippedFirstSlot == weaponList[i].id)
                    {
                        childTextTop.GetComponent<TextMeshProUGUI>().text = "EQUIPPED / UNEQUIP";
                        childTextTop.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                        childPrice.GetComponent<TextMeshProUGUI>().text = "";

                        childTextBottom.GetComponent<TextMeshProUGUI>().text = "EQUIP TO SLOT 2";
                        childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                    }
                    //SECOND SLOT
                    else if (equippedSecondSlot == weaponList[i].id)
                    {
                        childTextTop.GetComponent<TextMeshProUGUI>().text = "EQUIP TO SLOT 1";
                        childTextTop.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                        childPrice.GetComponent<TextMeshProUGUI>().text = "";

                        childTextBottom.GetComponent<TextMeshProUGUI>().text = "EQUIPPED / UNEQUIP";
                        childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                    }
                    else
                    {
                        childTextTop.GetComponent<TextMeshProUGUI>().text = "EQUIP TO SLOT 1";
                        childTextTop.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                        childPrice.GetComponent<TextMeshProUGUI>().text = "";

                        childTextBottom.GetComponent<TextMeshProUGUI>().text = "EQUIP TO SLOT 2";
                        childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                    }
                }

            }
            else
            {
                buttonTopObject.GetComponent<Button>().interactable = true;
                childTextTop.GetComponent<TextMeshProUGUI>().text = "Purchase";
                childTextTop.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                childPrice.GetComponent<TextMeshProUGUI>().text = "$" + weaponList[i].price;

                buttonBottomObject.GetComponent<Button>().interactable = false;
                childTextBottom.GetComponent<TextMeshProUGUI>().text = "NOT PURCHASED";
                childTextBottom.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 69);
            }

        }*/
        #endregion
    }

    //REFRESHES THE AMMO COUNTER AND AMMO PANEL
    //NOTE: AMMO COST PER STACK NOT INSIDE AMMO CLASS, STILL MANUAL
    void RefreshAmmoPanel()
    {
        //DYNAMICALLY MODIFY THE NAME AND PRICE DEPENDING ON THE AMMO TYPE
        //LIGHT = 30    , $100
        //SHOTGUN = 10  , $50
        //HEAVY = 15    , $200
        Transform textAmmoStack = buttonPurchaseAmmo.transform.Find("Text");
        Transform textAmmoStackPrice = buttonPurchaseAmmo.transform.Find("Price");

        if (weaponList[currentIndex].ammoType == AmmoType.LIGHT)
        {
            textAmmoStack.GetComponent<TextMeshProUGUI>().text = "BUY AMMO X30";
            textAmmoStackPrice.GetComponent<TextMeshProUGUI>().text = "$100";

            ammoCount.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.LoadedGameData.ammo[weaponList[currentIndex].ammoType].amount.ToString();
        }
        else if (weaponList[currentIndex].ammoType == AmmoType.SHOTGUN)
        {
            textAmmoStack.GetComponent<TextMeshProUGUI>().text = "BUY AMMO X10";
            textAmmoStackPrice.GetComponent<TextMeshProUGUI>().text = "$50";

            ammoCount.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.LoadedGameData.ammo[weaponList[currentIndex].ammoType].amount.ToString();
        }
        else if (weaponList[currentIndex].ammoType == AmmoType.HEAVY)
        {
            textAmmoStack.GetComponent<TextMeshProUGUI>().text = "BUY AMMO X15";
            textAmmoStackPrice.GetComponent<TextMeshProUGUI>().text = "$200";

            ammoCount.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.LoadedGameData.ammo[weaponList[currentIndex].ammoType].amount.ToString();
        }
        else if (weaponList[currentIndex].ammoType == AmmoType.NONE)
        {
            textAmmoStack.GetComponent<TextMeshProUGUI>().text = "IT'S MELEE...";
            textAmmoStackPrice.GetComponent<TextMeshProUGUI>().text = "$0";
        }

        //REFRESHES AMMO COUNT
        //ammoCount.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.LoadedGameData.ammo[weaponList[currentIndex].ammoType].amount.ToString();
    }
    #endregion

    #region Public Methods
    //Equip or unequip weapon
    public void Equip()
    {
        //IS IT MELEE OR RANGED
        //CHECK IF IT WANTS TO BE EQUIPPED OR UNEQUIPPED
        //IT DOES THIS BY LOOKING AT WHETHER OR NOT 

        if (weaponList[currentIndex].isOwned)
        {
            if (weaponList[currentIndex].ammoType == AmmoType.NONE)
            {
                GameManager.Instance.LoadedGameData.equippedMeleeWeapon = weaponList[currentIndex].id;
            }
            else
            {
                //DETERMINE IF THE PLAYER WANTS TO UNEQUIP, OR EQUIP ON EITHER SLOT
                //FIRST, DETERMINE THE BUTTON THAT WAS PRESSED (WAS IT THE TOP OR BOTTOM)
                //THEN CHECK WHETHER OR NOT IF WEAPON IS EQUIPPED ON EITHER SLOTS
                //BUTTON TOP = FIRST SLOT
                //BUTTON BOT = SECOND SLOT
                //IF NOT EQUIPPED ON EITHER, EQUIP IT TO THE SELECTED BUTTON
                //IF EQUIPPED, UNEQUIP
                //WHEN DONE, REFRESH

                if(EventSystem.current.currentSelectedGameObject.name == "Button_Top")
                {
                    Debug.Log("Top Gear"); 
                    //IF SAME WEAPON IS ALREADY EQUIPPED ON SAME SLOT, UNEQUIP
                    if (GameManager.Instance.LoadedGameData.equippedRangedWeapon1 == weaponList[currentIndex].id)
                    {
                        GameManager.Instance.LoadedGameData.equippedRangedWeapon1 = WeaponID.NONE;
                    }
                    //IF WEAPON IS ON OTHER SLOT
                    else if(GameManager.Instance.LoadedGameData.equippedRangedWeapon2 == weaponList[currentIndex].id)
                    {
                        GameManager.Instance.LoadedGameData.equippedRangedWeapon1 = weaponList[currentIndex].id;
                        GameManager.Instance.LoadedGameData.equippedRangedWeapon2 = WeaponID.NONE;
                    }
                    //IF WEAPON DOES NOT EXIST ON EITHER SLOT
                    else
                    {
                        GameManager.Instance.LoadedGameData.equippedRangedWeapon1 = weaponList[currentIndex].id;
                    }
                }
                else if (EventSystem.current.currentSelectedGameObject.name == "Button_Bottom")
                {
                    Debug.Log("Bottom Gear");
                    //IF SAME WEAPON IS ALREADY EQUIPPED ON SAME SLOT, UNEQUIP
                    if (GameManager.Instance.LoadedGameData.equippedRangedWeapon2 == weaponList[currentIndex].id)
                    {
                        GameManager.Instance.LoadedGameData.equippedRangedWeapon2 = WeaponID.NONE;
                    }
                    //IF WEAPON IS ON OTHER SLOT
                    else if (GameManager.Instance.LoadedGameData.equippedRangedWeapon1 == weaponList[currentIndex].id)
                    {
                        GameManager.Instance.LoadedGameData.equippedRangedWeapon2 = weaponList[currentIndex].id;
                        GameManager.Instance.LoadedGameData.equippedRangedWeapon1 = WeaponID.NONE;
                    }
                    //IF WEAPON DOES NOT EXIST ON EITHER SLOT
                    else
                    {
                        GameManager.Instance.LoadedGameData.equippedRangedWeapon2 = weaponList[currentIndex].id;
                    }
                }
            }
            PlayClick();
            Refresh();
        }
        
    }

    #region Cycle Methods
    public void CycleLeft()
    {
        currentIndex--;

        //Make sure that array index is not below 0
        if (currentIndex < 0)
        {
            currentIndex = weaponList.Length - 1;
        }
        PlayClick();
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
        PlayClick();
        Refresh();
    }
    #endregion

    #region Weapon Purchase Methods
    public void OpenWeaponPurchasePanel()
    {
        if(weaponList[currentIndex].isOwned != true)
        {
            PlayClick();
            purchasePanel.SetActive(true);
            Transform weaponText = purchasePanel.transform.Find("PurchasedWeaponName");
            weaponText.GetComponent<TextMeshProUGUI>().text = weaponList[currentIndex].id.ToString().Replace('_', ' ');

            Transform weaponPriceText = purchasePanel.transform.Find("Button_Purchase/Price");
            weaponPriceText.GetComponent<TextMeshProUGUI>().text = "$" + weaponList[currentIndex].price;
        }
    }

    public void ConfirmWeaponPurchase()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Purchase");
        weaponList[currentIndex].isOwned = true;
        GameManager.Instance.LoadedGameData.money = (float)GameManager.Instance.LoadedGameData.money - (float)weaponList[currentIndex].price;
        Refresh();
        Debug.Log("Purchased weapon " + weaponList[currentIndex].id.ToString());
        CloseWeaponPurchasePanel();
    }

    public void CloseWeaponPurchasePanel()
    {
        PlayClick();
        purchasePanel.SetActive(false);
    }
    #endregion

    #region Ammo Purchase Methods
    public void OpenAmmoPurchasePanel()
    {
        PlayClick();
        ammoPurchasePanel.SetActive(true);
        Transform ammoText = ammoPurchasePanel.transform.Find("PurchasedAmmoName");
        ammoText.GetComponent<TextMeshProUGUI>().text = weaponList[currentIndex].ammoType.ToString() + " AMMO";
    }

    public void ConfirmPurchaseAmmo()
    {
        if(weaponList[currentIndex].ammoType == AmmoType.LIGHT)
        {
            GameManager.Instance.LoadedGameData.ammo[weaponList[currentIndex].ammoType].amount += 30;
        }
        else if(weaponList[currentIndex].ammoType == AmmoType.SHOTGUN)
        {
            GameManager.Instance.LoadedGameData.ammo[weaponList[currentIndex].ammoType].amount += 10;
        }
        else if (weaponList[currentIndex].ammoType == AmmoType.HEAVY)
        {
            GameManager.Instance.LoadedGameData.ammo[weaponList[currentIndex].ammoType].amount += 15;
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Purchase");
        Refresh();
        CloseAmmoPurchasePanel();
    }

    public void CloseAmmoPurchasePanel()
    {
        PlayClick();
        ammoPurchasePanel.SetActive(false);
    }
    #endregion

    //Play sound when hovering
    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }
    #endregion
}
