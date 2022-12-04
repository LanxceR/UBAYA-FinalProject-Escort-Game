using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemPickupScript))]
public class ItemPickupMedkit : MonoBehaviour, IItemPickup
{
    [Header("Properties")]
    [SerializeField]
    private float healAmount;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void OnPickup(GameObject picker)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Pickup/PickupHealth");
        // Fetch victim's health on their parent gameobject
        HealthScript health = Utilities.FindParentOfType<HealthScript>(picker.transform, out _);

        if (health)
        {
            if (health.IsOnMaxHealth()) return;
            health.Heal(healAmount);
        }

        Destroy(gameObject);
    }
}
