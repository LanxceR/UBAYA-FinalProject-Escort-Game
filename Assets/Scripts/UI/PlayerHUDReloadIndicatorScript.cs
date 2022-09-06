using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The HUD reload indicator script (handles the reload indicator HUD)
/// </summary>
public class PlayerHUDReloadIndicatorScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    internal PlayerHUDScript pHUDScript;

    // TODO: Implement accessing playerWeaponScript programmatically (through a game manager)
    // Components
    [SerializeField]
    internal Slider reloadIndicatorSlider;
    [SerializeField]
    internal WeaponScript playerWeaponScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HUDReloadIndicatorScript starting");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerWeaponScript.reloadElapsedTime > 0)
        {
            // Player is reloading

            // Enable slider gameobject
            reloadIndicatorSlider.gameObject.SetActive(true);

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
}
