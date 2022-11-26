using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main escortee ally script (or the hub)
/// </summary>
public class AllyEscorteeScript : MonoBehaviour
{
    // Reference to the parent escortee script
    [SerializeField]
    private EscorteeScript escorteeScript;

    // References of the escortee ally's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal AllyEscorteeAnimationScript allyEscortAnimationScript;
    [SerializeField]
    internal AllyEscorteeAttackScript allyEscortAttackScript;
    // Generic sub-scripts
    [SerializeField]
    internal HealthScript healthScript;
    [SerializeField]
    internal SeekTargetScript seekTargetScript;

    // Start is called before the first frame update
    void Start()
    {
        // Set health
        healthScript.MaxHealth = 999f;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
