using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDAmmoScript : MonoBehaviour
{
    public Image ammoBar;
    public Image ammoBarBlock;
    public TextMeshProUGUI currentAmmoText;
    public TextMeshProUGUI totalAmmoText;

    private WeaponScript playerWeaponScript;
    private PlayerScript activePlayer;

    float currentAmmo;
    float magSize;
    float totalAmmo;
    float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        lerpSpeed = 7f * Time.deltaTime;
        activePlayer = GameManager.Instance.gamePlayer.ActivePlayer;

        if (activePlayer)
        {
            // Add listener to Inventory's OnEquipmentSwitch UnityEvent         
            activePlayer.inventoryScript.OnEquipmentSwitch?.AddListener(AssignWeaponScript);
        }

        RefreshAmmoValues();
    }

    void RefreshAmmoValues()
    {
        if (playerWeaponScript)
        {
            if (playerWeaponScript.weaponAmmoScript.loadedAmmo == Mathf.Infinity)
            {
                ammoBar.enabled = false;
                currentAmmoText.enabled = false;
                totalAmmoText.enabled = false;

                ammoBarBlock.enabled = false;
            }
            else
            {
                ammoBar.enabled = true;
                ammoBarBlock.enabled = true;

                currentAmmoText.enabled = true;
                totalAmmoText.enabled = true;

                totalAmmo = GameManager.Instance.LoadedGameData.ammo[playerWeaponScript.ammoType].Amount;
                currentAmmo = playerWeaponScript.weaponAmmoScript.loadedAmmo;

                totalAmmoText.text = "/" + totalAmmo.ToString();
                currentAmmoText.text = currentAmmo.ToString();

                magSize = playerWeaponScript.ammoMagSize;

                if(currentAmmo == 0)
                {
                    totalAmmoText.color = new Color32(255, 255, 255, 69);
                }
                else
                {
                    totalAmmoText.color = new Color32(255, 255, 255, 255);
                }

                CircleBarFiller();
            }
        }
    }

    void CircleBarFiller()
    {
        ammoBar.fillAmount = Mathf.Lerp(ammoBar.fillAmount, currentAmmo / magSize, lerpSpeed);
        Color barColor = Color.Lerp(Color.red, Color.white, (currentAmmo / magSize));
        ammoBar.color = barColor;
    }



    internal void AssignWeaponScript()
    {
        InventoryScript inv = GameManager.Instance.gamePlayer.ActivePlayer.inventoryScript;

        // Assign to show currently equipped item
        playerWeaponScript = inv.GetCurrentEquippedItem() as WeaponScript;
    }


    // Update is called once per frame
    void Update()
    {
        RefreshAmmoValues();
    }
}
