using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemPickupScript))]
public class ItemPickupAmmo : MonoBehaviour, IItemPickup
{
    [Header("Properties")]
    [SerializeField]
    private AmmoType ammoType;
    [SerializeField]
    private float amount;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void OnPickup(GameObject picker)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Pickup/PickupAmmo");
        GameManager.Instance.LoadedGameData.ammo[ammoType].Amount += amount;

        Destroy(gameObject);
    }
}
