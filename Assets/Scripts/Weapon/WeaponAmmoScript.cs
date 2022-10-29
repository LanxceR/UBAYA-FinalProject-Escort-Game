using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// The weapon ammo script (handles all ammo related actions for weapons)
/// </summary>
[RequireComponent(typeof(WeaponScript))]
public class WeaponAmmoScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    internal WeaponScript weaponScript;

    [Header("Misc Settings")]
    [SerializeField] private bool drawGizmo = false;

    // Variables
    [SerializeField]
    internal float loadedAmmo;

    // Start is called before the first frame update
    void Start()
    {
        // Set ammo count at the start
        loadedAmmo = weaponScript.ammoMagSize;
    }

    internal void SaveLoadedAmmoToPlayerData()
    {
        GameManager.Instance.LoadedPlayerData.ammo[weaponScript.ammoType].amount += loadedAmmo;
    }

    // To update ammo count
    internal void SetLoadedAmmo(float valueToSetAt)
    {
        loadedAmmo = valueToSetAt;
    }

    // To update ammo count
    internal void ChangeLoadedAmmo(float value)
    {
        loadedAmmo += value;
    }


    // To update ammo count
    internal void ReloadLoadedAmmo(bool takeFromReserveAmmo)
    {
        float amountToAdd = weaponScript.ammoMagSize - loadedAmmo;

        if (takeFromReserveAmmo)
        {
            if (weaponScript.ammoType == AmmoType.NONE) return;

            amountToAdd = amountToAdd > GameManager.Instance.LoadedPlayerData.ammo[weaponScript.ammoType].amount ? 
                GameManager.Instance.LoadedPlayerData.ammo[weaponScript.ammoType].amount :
                amountToAdd;

            GameManager.Instance.LoadedPlayerData.ammo[weaponScript.ammoType].amount -= amountToAdd;

            loadedAmmo += amountToAdd;
        }
    }
    internal void ReloadLoadedAmmo(float value, bool takeFromReserveAmmo)
    {
        value = loadedAmmo + value >= weaponScript.ammoMagSize ? weaponScript.ammoMagSize - loadedAmmo : value;

        if (takeFromReserveAmmo)
        {
            if (weaponScript.ammoType == AmmoType.NONE) return;

            value = value > GameManager.Instance.LoadedPlayerData.ammo[weaponScript.ammoType].amount ?
                GameManager.Instance.LoadedPlayerData.ammo[weaponScript.ammoType].amount :
                value;

            GameManager.Instance.LoadedPlayerData.ammo[weaponScript.ammoType].amount -= value;

            loadedAmmo += value;
        }
    }

#if UNITY_EDITOR
    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Vector3 textRootPos = transform.position + new Vector3(0.2f, 0.2f);
        int textPos = 0;

        // Ammo count (as text)
        Vector3 ammoTextPos = textRootPos + new Vector3(0f, 0.2f * textPos);
        Handles.Label(ammoTextPos, $"Ammo: {loadedAmmo.ToString()}");
        textPos++;

        // Reloading (as text & completion percentage)
        Vector3 reloadTextPos = textRootPos + new Vector3(0f, 0.2f * textPos);
        if (weaponScript.reloadElapsedTime > 0)
        {
            Handles.Label(reloadTextPos, $"Reloading...{weaponScript.reloadProgress}%");
            textPos++;
        }
    }
#endif
}
