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

    // TODO: Implement accessing playerWeaponScript programmatically (through a game manager)
    // Components
    [SerializeField]
    private TextMeshProUGUI ammoText;
    [SerializeField]
    private WeaponScript playerWeaponScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HUDAmmoScript starting");
    }

    // Update is called once per frame
    void Update()
    {
        // Update ammo counter text
        ammoText.text = $"{playerWeaponScript.Ammo} / {playerWeaponScript.startingAmmo}";
    }
}
