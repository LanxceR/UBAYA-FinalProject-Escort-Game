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
    private GameObject inventoryHolder;

    // Variables
    internal List<IEquipmentItem> equipments;

    [SerializeField]
    internal int equippedItemIndex;

    #region INPUTS
    // OnPSwitchEquipment listener from InputAction "MainPlayerInput.inputaction"
    void OnPSwitchEquipment()
    {
        // Determine the next weapon index to cycle
        // If equipped index is the last equipment in inventory, go back to 0
        // Otherwise, add 1 to equipped index item (cycle to the next weapon)
        int nextWeaponIndex = equippedItemIndex >= equipments.Count - 1 ? 0 : equippedItemIndex + 1;

        // Switch equipment
        SwitchEquipment(nextWeaponIndex);

        OnEquipmentSwitch.Invoke();
    }
    #endregion

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Fill in inventory at start
        UpdateInventory();

        // Switch to the first equipment
        SwitchEquipment(0);
    }

    private void UpdateInventory()
    {
        equipments = inventoryHolder.GetComponentsInChildren<IEquipmentItem>(true).ToList();
    }

    internal IEquipmentItem GetCurrentEquippedItem()
    {
        return GetCurrentEquippedItem(equippedItemIndex);
    }
    internal IEquipmentItem GetCurrentEquippedItem(int index)
    {
        return equipments[index];
    }

    // TODO: (DUPLICATE) Finish implementing weapon switching and HUD weapon script assigning.
    internal void SwitchEquipment(int index)
    {
        // Use try catch to prevent index out of bounds exception
        try
        {
            GameObject obj = null;

            // Go through equipments list
            for (int i = 0; i < equipments.Count; i++)
            {
                // Fetch selected equipment game object
                if (equipments[i] is WeaponScript) obj = (equipments[i] as WeaponScript).gameObject;

                // Activate selected equipment
                if (i == index)
                {
                    equippedItemIndex = index;
                    obj.SetActive(true);
                }

                // Deactivate everything else
                else obj.SetActive(false);
            }
        }
        catch (IndexOutOfRangeException)
        {
            return;
        }
        catch (Exception e)
        {
            Debug.LogError("Selected item index not found \n" + e);
            return;
        }
    }
}
