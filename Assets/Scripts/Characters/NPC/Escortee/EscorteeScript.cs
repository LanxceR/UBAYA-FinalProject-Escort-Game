using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EscorteeID
{
    BUS,
    PICKUP_TRUCK,
    MILITARY_TRUCK
}

/// <summary>
/// The main escortee script (or the hub)
/// </summary>
[System.Serializable]
public class EscorteeScript : MonoBehaviour, ICharacter
{
    // Weapon type
    [Header("Escortee ID")]
    [SerializeField]
    internal EscorteeID id;

    // Escortee stats
    [Header("Escortee Stats")]
    [SerializeField]
    internal float health = 5f;
    [SerializeField]
    internal float maxSpeed = 3f;
    [SerializeField]
    internal float acceleration = 0.2f;
    [SerializeField]
    internal float deceleration = 0.5f; // Otherwise known as braking power

    // References of the player's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal EscorteeInputScript escorteeInputScript;
    [SerializeField]
    internal EscorteeMovementScript escorteeMovementScript;
    [SerializeField]
    internal EscorteeAnimationScript escorteeAnimationScript;
    [SerializeField]
    internal EscorteeInteractScript escorteeInteractScript;
    // General sub-scripts
    [SerializeField]
    internal HealthScript healthScript;
    [SerializeField]
    internal EmitAggroScript emitAggroScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main EscorteeScript starting");

        // Set health
        healthScript.MaxHealth = health;

        // Add listener to Health's OnHealthReachedZero UnityEvent
        healthScript.OnHealthReachedZero.AddListener(EscorteeDeath);
    }

    void EscorteeDeath()
    {
        if (escorteeInputScript)
            escorteeInputScript.enabled = false;

        if (escorteeInteractScript)
            escorteeInteractScript.enabled = false;

        if (emitAggroScript)
            emitAggroScript.enabled = false;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
