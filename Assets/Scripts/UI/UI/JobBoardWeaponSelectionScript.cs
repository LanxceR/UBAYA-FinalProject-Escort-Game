using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class JobBoardWeaponSelectionScript : MonoBehaviour
{
    WeaponScript[] weaponList;
    private int currentIndex;
    public GameObject ammoPurchasePanel;

    //GAMEOBJECTS
    #region Attached GameObjects

    //General Info
    public TextMeshProUGUI nameObject;
    public GameObject imageObject;
    public float weaponSlot;

    [Header("Stats Texts")]
    public GameObject parameterOneObject;
    public GameObject parameterTwoObject;

    [Header("Ammo Panel")]
    public GameObject ammoPanel;
    public GameObject ammoCount;
    public GameObject buttonPurchaseAmmo;

    #endregion

    WeaponScript[] weaponListForSlots;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        weaponList = GameManager.Instance.gameWeapon.weaponPrefabs;

        WeaponID equippedWeapon;

        //CREATE PRIVATE ARRAY OR LIST OF WEAPONS FOR ONE SLOT
        if(weaponSlot == 1 || weaponSlot == 2)
        {
            if (weaponSlot == 1)
            {
                equippedWeapon = GameManager.Instance.LoadedGameData.equippedRangedWeapon1;
            }
            else
            {
                equippedWeapon = GameManager.Instance.LoadedGameData.equippedRangedWeapon2;
            }

            foreach (WeaponScript w in weaponList)
            {
                if (w.ammoType != AmmoType.NONE)
                {
                    weaponListForSlots[currentIndex] = w.gameObject.AddComponent<WeaponScript>();
                    currentIndex++;
                }
            }
            weaponListForSlots[4] = gameObject.AddComponent<WeaponScript>();
        }
        else
        {
            equippedWeapon = GameManager.Instance.LoadedGameData.equippedMeleeWeapon;
            imageObject.transform.Rotate(0,0,-90);

            foreach (WeaponScript w in weaponList)
            {
                if (w.ammoType == AmmoType.NONE)
                {
                    weaponListForSlots[currentIndex] = w.gameObject.AddComponent<WeaponScript>();
                    currentIndex++;
                }
            }
            weaponListForSlots[4] = gameObject.AddComponent<WeaponScript>();
        }

        for (int i = 0; i < weaponListForSlots.Length; i++)
        {
            if (weaponListForSlots[i].id == equippedWeapon)
            {
                Transform childImage = weaponList[i].transform.Find("Weapon Model");

                nameObject.GetComponent<TextMeshProUGUI>().text = weaponListForSlots[i].id.ToString().Replace('_', ' ');

                imageObject.GetComponent<Image>().sprite = childImage.GetComponent<SpriteRenderer>().sprite;

                currentIndex = i;

                break;
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
        currentIndex--;

        //Make sure that array index is not below 0
        if (currentIndex < 0)
        {
            currentIndex = weaponListForSlots.Length - 1;
        }

        Refresh();
    }

    public void CycleRight()
    {
        currentIndex++;

        //Make sure that array index is not below 0
        if (currentIndex > weaponListForSlots.Length - 1)
        {
            currentIndex = 0;
        }

        Refresh();
    }

    private void Refresh()
    {
        Transform childImage = weaponList[currentIndex].transform.Find("Weapon Model");

        if(currentIndex == 4)
        {
            nameObject.GetComponent<TextMeshProUGUI>().text = "None";

            imageObject.SetActive(false);
        }
        else
        {
            nameObject.GetComponent<TextMeshProUGUI>().text = weaponListForSlots[currentIndex].id.ToString().Replace('_', ' ');

            imageObject.GetComponent<Image>().sprite = childImage.GetComponent<SpriteRenderer>().sprite;
            imageObject.SetActive(true);
        }


        //RefreshSliders(currentIndex);
        //RefreshButtons();
        //RefreshAmmoPanel();
    }

    public void Purchase()
    {

    }
}
