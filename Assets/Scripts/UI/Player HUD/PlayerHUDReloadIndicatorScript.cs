using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The player attached HUD reload indicator script (handles the reload indicator HUD)
/// </summary>
public class PlayerHUDReloadIndicatorScript : MonoBehaviour
{
    // Reference to the main player HUD script
    [SerializeField]
    internal PlayerHUDScript pHUDScript;

    // Components
    [Header("UI Components")]
    [SerializeField]
    internal Slider reloadIndicatorSlider;
    [SerializeField]
    internal TextMeshProUGUI noAmmoAlertText;

    [Header("Weapon Component")]
    [SerializeField]
    internal WeaponScript playerWeaponScript;

    // Variables
    internal Coroutine alertCoroutine;

    // Variables
    private InventoryScript inv;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerHUDReloadIndicatorScript starting");
               
        // Add listener to OnEquipmentSwitch to assign the current active weapon script
        inv = Utilities.FindParentOfType<InventoryScript>(transform);
        if (inv)
            inv.OnEquipmentSwitch?.AddListener(AssignWeaponScript);

        // Assign weapon script at start
        AssignWeaponScript();

        // Add listener to NoAmmoAlert
        playerWeaponScript.weaponAmmoScript.NoAmmoAlert?.AddListener(NoAmmoAlert);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerWeaponScript.reloadElapsedTime > 0)
        {
            // Player is reloading

            // Enable slider gameobject
            reloadIndicatorSlider.gameObject.SetActive(true);

            // Interrupt any ongoing alert coroutine
            InterruptAlertCoroutine();

            // Set slider value
            reloadIndicatorSlider.value = playerWeaponScript.reloadProgress;
        }
        else
        {
            // Player is NOT reloading

            // Disable slider gameobject
            reloadIndicatorSlider.gameObject.SetActive(false);

            // Set slider value
            reloadIndicatorSlider.value = 0f;
        }
    }

    internal void AssignWeaponScript()
    {
        // Assign to show currently equipped item
        playerWeaponScript = inv.GetCurrentEquippedItem() as WeaponScript;
    }

    // To stop any ongoing reload
    internal void InterruptAlertCoroutine()
    {
        if (alertCoroutine != null)
        {
            StopCoroutine(alertCoroutine);
            alertCoroutine = null;

            noAmmoAlertText.alpha = 0;
        }
    }

    void NoAmmoAlert()
    {
        if (alertCoroutine == null)
        {
            // Show alert for 3 seconds, flash 3 times
            alertCoroutine = StartCoroutine(ShowNoAmmoAlert(3, 3));
        }
    }

    // Show alert (use flashAmount = 0 if you dont want any flashing/blinking behaviour)
    internal IEnumerator ShowNoAmmoAlert(float duration, int flashAmount)
    {
        if (flashAmount <= 0)
        {
            noAmmoAlertText.alpha = 1;
            yield return new WaitForSeconds(duration);
            noAmmoAlertText.alpha = 0;
        }
        else
        {
            float timeGap = (duration / flashAmount) / 2;

            for (int i = 0; i < flashAmount; i++)
            {
                noAmmoAlertText.alpha = 1;
                yield return new WaitForSeconds(timeGap);
                noAmmoAlertText.alpha = 0;
                yield return new WaitForSeconds(timeGap);
            }
        }

        alertCoroutine = null;
    }
}
