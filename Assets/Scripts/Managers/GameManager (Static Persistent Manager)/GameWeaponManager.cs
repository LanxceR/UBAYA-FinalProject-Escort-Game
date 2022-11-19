using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game weapons manager script
/// </summary>
public class GameWeaponManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;

    // Weapon Prefabs
    [Header("All Weapon Prefabs")]
    [SerializeField] internal WeaponScript[] weaponPrefabs; // List of ALL enemy prefabs possible to spawn

    #region Prefab Utilities
    // TODO: (DUPLICATE) Maybe put these methods in their corresponding scripts and load using Resources.Load
    /// <summary>
    /// Get a weapon instance of a type
    /// </summary>
    /// <param name="weaponType">The weapon type</param>
    /// <returns></returns>
    public WeaponScript GetWeapon(WeaponID weaponType)
    {
        // Find a weapon of a certain type
        foreach (WeaponScript w in weaponPrefabs)
        {
            if (w.id == weaponType) return w;
        }

        // If nothing is found, return null
        return null;
    }
    public void UpdateAllWeaponFlags()
    {
        foreach (WeaponScript w in weaponPrefabs)
        {
            // First set ALL weapons flags to false
            w.isOwned = false;
            w.isEquipped = false;
        }

        // Set all isOwned flags
        foreach (WeaponID w in gameManager.LoadedGameData.ownedWeapons)
        {
            // Then set owned isOwned flags to true
            GetWeapon(w).isOwned = true;
        }

        // Set all isEquipped flags
        WeaponID equippedWeapon;
        equippedWeapon = gameManager.LoadedGameData.equippedMeleeWeapon;
        GetWeapon(equippedWeapon).isEquipped = true;
        equippedWeapon = gameManager.LoadedGameData.equippedRangedWeapon1;
        GetWeapon(equippedWeapon).isEquipped = true;
        equippedWeapon = gameManager.LoadedGameData.equippedRangedWeapon2;
        GetWeapon(equippedWeapon).isEquipped = true;
    }
    public void SetWeaponOwnedFlag(WeaponID weaponType, bool isOwned)
    {
        GetWeapon(weaponType).isOwned = isOwned;
    }
    public void SetWeaponEquippedFlag(WeaponID weaponType, bool isEquipped)
    {
        GetWeapon(weaponType).isEquipped = isEquipped;
    }
    #endregion
}
