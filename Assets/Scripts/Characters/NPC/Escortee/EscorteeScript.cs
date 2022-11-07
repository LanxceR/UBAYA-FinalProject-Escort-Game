using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main escortee script (or the hub)
/// </summary>
public class EscorteeScript : MonoBehaviour, ICharacter
{
    // TODO: Implement hitching / hanging onto escortee

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

    // TODO: Implement escortee Health
    // References of the player's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal EscorteeInputScript escorteeInputScript;
    [SerializeField]
    internal EscorteeMovementScript escorteeMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main EscorteeScript starting");
    }
}
