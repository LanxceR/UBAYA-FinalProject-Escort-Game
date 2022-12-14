using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InventoryScript : MonoBehaviour
{
    // Events
    [Header("Events")]
    internal UnityEvent OnEquipmentSwitch = new UnityEvent();

    // Components
    [Header("Components")]
    [SerializeField]
    internal GameObject inventoryHolder;

    // Variables
    [SerializeField]
    internal List<IEquipmentItem> equipments;

    [SerializeField]
    internal int heldItemIndex = 0;

    #region INPUTS
    // OnPSwitchEquipment listener from InputAction "MainPlayerInput.inputaction"
    void OnPSwitchEquipment()
    {
        if (!GameManager.Instance.GameIsPlaying || !this.enabled) return;

        // Determine the next weapon index to cycle
        // If equipped index is the last equipment in inventory, go back to 0
        // Otherwise, add 1 to equipped index item (cycle to the next weapon)
        int nextWeaponIndex = heldItemIndex >= equipments.Count - 1 ? 0 : heldItemIndex + 1;

        // Switch equipment
        SwitchEquipment(nextWeaponIndex);
    }

    // OnPWeapon1 listener from InputAction "MainPlayerInput.inputaction"
    void OnPWeapon1()
    {
        if (!GameManager.Instance.GameIsPlaying || !this.enabled) return;

        // Switch to equipment 1 (index = 0)
        SwitchEquipment(0);
    }

    // OnPWeapon1 listener from InputAction "MainPlayerInput.inputaction"
    void OnPWeapon2()
    {
        if (!GameManager.Instance.GameIsPlaying || !this.enabled) return;

        // Switch to equipment 2 (index = 1)
        SwitchEquipment(1);
    }

    // OnPWeapon1 listener from InputAction "MainPlayerInput.inputaction"
    void OnPWeapon3()
    {
        if (!GameManager.Instance.GameIsPlaying || !this.enabled) return;

        // Switch to equipment 3 (index = 2)
        SwitchEquipment(2);
    }
    #endregion

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Fill in inventory at start
        UpdateInventory();

        heldItemIndex = 9999; // Initial value to allow equipping on start
        // Switch to the first equipment
        SwitchEquipment(0);
    }

    internal void UpdateInventory()
    {
        equipments = inventoryHolder.GetComponentsInChildren<IEquipmentItem>(true).ToList();
    }

    internal void DisableAllEquipment()
    {
        try
        {
            foreach (var e in equipments)
            {
                if (e is WeaponScript) (e as WeaponScript).gameObject.SetActive(false);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Selected item index not found \n" + e);
            return;
        }
    }

    internal IEquipmentItem GetCurrentEquippedItem()
    {
        return GetCurrentEquippedItem(heldItemIndex);
    }
    internal IEquipmentItem GetCurrentEquippedItem(int index)
    {
        return equipments[index];
    }

    internal void SwitchEquipment(int index)
    {
        // Use try catch to prevent index out of bounds exception
        try
        {
            // If player attempts to switch to an empty inventory slot, do nothing
            if (index > equipments.Count - 1) return;

            // If player changes to the currently equipped weapon, do nothing
            if (heldItemIndex == index) return;

            GameObject obj = null;

            FMOD.Studio.EventInstance weaponSwitch = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Weapon/WeaponSwitch");

            // Go through equipments list
            for (int i = 0; i < equipments.Count; i++)
            {
                // Fetch selected equipment game object
                if (equipments[i] is WeaponScript) obj = (equipments[i] as WeaponScript).gameObject;

                // Activate selected equipment
                if (i == index)
                {
                    heldItemIndex = index;
                    obj.SetActive(true);

                    OnEquipmentSwitch.Invoke();

                    if(i == 0)
                    {
                        weaponSwitch.setParameterByName("SwitchType", 0);
                    }
                    else
                    {
                        weaponSwitch.setParameterByName("SwitchType", 1);
                    }

                    weaponSwitch.start();
                }

                // Deactivate everything else
                else obj.SetActive(false);
            }
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError("Selected item index out of range \n" + 
                            $"Equipment 0: {equipments[0].GetGameObject().name} \n" +
                            $"Equipment 1: {equipments[1].GetGameObject().name} \n" +
                            $"Equipment 2: {equipments[2].GetGameObject().name} \n" +
                            e);
            return;
        }
        catch (Exception e)
        {
            Debug.LogError("Selected item index not found \n" + e);
            return;
        }
    }
}
