using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The HUD script (handles the ammo counter HUD)
/// </summary>
public class HUDAmmoScript : MonoBehaviour
{
    // Reference to the main HUD script
    [SerializeField]
    internal HUDScript hudScript;

    // TODO: Implement accessing playerWeaponScript programmatically (through a game manager)
    // Components
    [SerializeField]
    internal TextMeshProUGUI ammoText;
    [SerializeField]
    internal WeaponScript playerWeaponScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HUDAmmoScript starting");
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = $"{playerWeaponScript.Ammo} / {playerWeaponScript.startingAmmo}";
    }
}
