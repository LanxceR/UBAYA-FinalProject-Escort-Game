using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The ammo HUD script (handles the ammo counter HUD)
/// </summary>
public class HUDAmmoScript : MonoBehaviour
{
    // Reference to the main HUD script
    [SerializeField]
    private HUDScript hudScript;

    // Components
    [SerializeField]
    private TextMeshProUGUI ammoText;
    [SerializeField]
    private WeaponScript playerWeaponScript;

    // Variables
    private PlayerScript activePlayer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HUDAmmoScript starting");

        activePlayer = GameManager.Instance.ActivePlayer;

        if (activePlayer)
        {
            // Add listener to Inventory's OnEquipmentSwitch UnityEvent         
            activePlayer.inventoryScript.OnEquipmentSwitch?.AddListener(AssignWeaponScript);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerWeaponScript)
        {
            string textToDisplay;

            if (playerWeaponScript.weaponAmmoScript.loadedAmmo == Mathf.Infinity)
            {
                textToDisplay = "INF";
            }
            else
            {
                textToDisplay = $"{playerWeaponScript.weaponAmmoScript.loadedAmmo} / ";

                if (playerWeaponScript.ammoType == AmmoType.NONE)
                {
                    textToDisplay = "N/A";
                    return;
                }

                textToDisplay += $"{GameManager.Instance.LoadedGameData.ammo[playerWeaponScript.ammoType].amount}";
            }

            // Update ammo counter text
            ammoText.text = textToDisplay;
        }
    }

    internal void AssignWeaponScript()
    {
        InventoryScript inv = GameManager.Instance.ActivePlayer.inventoryScript;

        // Assign to show currently equipped item
        playerWeaponScript = inv.GetCurrentEquippedItem() as WeaponScript;
    }
}
