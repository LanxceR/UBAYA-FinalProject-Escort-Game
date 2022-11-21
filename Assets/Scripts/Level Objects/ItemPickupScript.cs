using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The item pickup script
/// </summary>
public class ItemPickupScript : MonoBehaviour
{
    internal IItemPickup itemBehaviour;

    // Generic sub-scripts
    [Header("Collision")]
    [SerializeField]
    internal CollisionScript collisionScript;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        itemBehaviour = GetComponent<IItemPickup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add listener to collision UnityEvents
        collisionScript.OnCollisionEnterGO?.AddListener(OnPickup);
    }

    private void OnPickup(GameObject picker)
    {
        itemBehaviour.OnPickup(picker);
    }
}
