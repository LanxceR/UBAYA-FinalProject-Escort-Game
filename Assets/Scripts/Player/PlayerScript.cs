using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main player script (or the hub)
/// </summary>
[RequireComponent(typeof(Moveable))]
public class PlayerScript : MonoBehaviour
{
    // Player stats
    [Header("Movement Settings")]
    [SerializeField]
    internal float Speed = 5f;

    // References of the player's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal PlayerMovementScript playerMovementScript;
    [SerializeField]
    internal PlayerInputScript playerInputScript;
    [SerializeField]
    internal PlayerAnimationScript playerAnimationScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main PlayerScript starting");
    }
}
