using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    // Components
    [Header("Components")]
    [SerializeField]
    private GameObject inventoryHolder;

    // Variables
    private List<IEquipmentItem> equipments;

    [SerializeField]
    internal IEquipmentItem currentEquippedItem;

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

    // TODO: (DUPLICATE) Finish implementing weapon switching and HUD weapon script assigning.
    internal void SwitchEquipment(int index)
    {
        // Use try catch to prevent index out of bounds exception
        try
        {
            GameObject obj = new GameObject();

            // Go through equipments list
            for (int i = 0; i < equipments.Count; i++)
            {
                // Fetch selected equipment game object
                if (equipments[i] is WeaponScript) obj = (equipments[i] as WeaponScript).gameObject;

                // Activate selected equipment
                if (i == index)
                {
                    currentEquippedItem = equipments[index];
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
