using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The escortee ally holder script (holds & manages the escortee ally shooter)
/// </summary>
public class EscorteeAllyHolderScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private EscorteeScript escorteeScript;

    // Component
    [Header("Ally")]
    [SerializeField]
    internal AllyEscorteeScript ally;
}
