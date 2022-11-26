using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

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

    // Events
    [Header("Events")]
    internal UnityEvent NoAmmoAlert = new UnityEvent();

    // Variables
    [SerializeField]
    internal float loadedAmmo;
    internal Coroutine reloadCoroutine;

    private InventoryScript inv;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Set ammo count at the start
        loadedAmmo = weaponScript.ammoMagSize;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add listener to OnEquipmentSwitch to interrupt reloads on equipment switch
        inv = Utilities.FindParentOfType<InventoryScript>(transform, out _);
        if (inv)
            inv.OnEquipmentSwitch?.AddListener(InterruptReloadCoroutine);
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        if (weaponScript.weaponInputScript.Input_Reload == 1)
        {
            if (GameManager.Instance.LoadedGameData.ammo[weaponScript.ammoType].Amount <= 0)
            {
                // Invoke no ammo alert event
                NoAmmoAlert?.Invoke();
            }
            else if (loadedAmmo < weaponScript.ammoMagSize && reloadCoroutine == null)
            {
                // Reload
                reloadCoroutine = StartCoroutine(ReloadCoroutine(weaponScript.reloadTime));
            }
        }

        if (weaponScript.weaponInputScript.Input_Attack == 1)
        {
            if (loadedAmmo <= 0 && reloadCoroutine == null)
            {
                if (GameManager.Instance.LoadedGameData.ammo[weaponScript.ammoType].Amount <= 0)
                {
                    // Invoke no ammo alert event
                    NoAmmoAlert?.Invoke();
                }
                else
                {
                    // If player attacks while ammo is depleted, perform a reload instead
                    reloadCoroutine = StartCoroutine(ReloadCoroutine(weaponScript.reloadTime));
                }
            }
        }
    }

    internal void SaveLoadedAmmoToPlayerData()
    {
        GameManager.Instance.LoadedGameData.ammo[weaponScript.ammoType].Amount += loadedAmmo;
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

            amountToAdd = amountToAdd > GameManager.Instance.LoadedGameData.ammo[weaponScript.ammoType].Amount ? 
                GameManager.Instance.LoadedGameData.ammo[weaponScript.ammoType].Amount :
                amountToAdd;

            GameManager.Instance.LoadedGameData.ammo[weaponScript.ammoType].Amount -= amountToAdd;

            loadedAmmo += amountToAdd;
        }
        else
        {
            if (weaponScript.ammoType == AmmoType.NONE) return;

            loadedAmmo += amountToAdd;
        }
    }
    internal void ReloadLoadedAmmo(float value, bool takeFromReserveAmmo)
    {
        value = loadedAmmo + value >= weaponScript.ammoMagSize ? weaponScript.ammoMagSize - loadedAmmo : value;

        if (takeFromReserveAmmo)
        {
            if (weaponScript.ammoType == AmmoType.NONE) return;

            value = value > GameManager.Instance.LoadedGameData.ammo[weaponScript.ammoType].Amount ?
                GameManager.Instance.LoadedGameData.ammo[weaponScript.ammoType].Amount :
                value;

            GameManager.Instance.LoadedGameData.ammo[weaponScript.ammoType].Amount -= value;

            loadedAmmo += value;
        }
        else
        {
            if (weaponScript.ammoType == AmmoType.NONE) return;

            loadedAmmo += value;
        }
    }

    // To stop any ongoing reload
    internal void InterruptReloadCoroutine()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
            weaponScript.reloadElapsedTime = 0f;
            weaponScript.reloadProgress = 0f;
        }
    }

    // Weapon reload
    internal IEnumerator ReloadCoroutine(float reloadTime)
    {
        weaponScript.reloadElapsedTime = 0f;
        weaponScript.reloadProgress = 0f;

        while (weaponScript.reloadElapsedTime < reloadTime)
        {
            weaponScript.reloadElapsedTime += Time.deltaTime;
            weaponScript.reloadProgress = (weaponScript.reloadElapsedTime / reloadTime * 100);

            yield return null;
        }

        // Reload weapon's ammo
        ReloadLoadedAmmo(true);

        weaponScript.reloadElapsedTime = 0f;
        weaponScript.reloadProgress = 0f;

        reloadCoroutine = null;
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
