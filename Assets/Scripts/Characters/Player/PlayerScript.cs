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
    [Header("Player Stats")]
    [SerializeField]
    internal float health = 5f;
    [SerializeField]
    internal float speed = 1f;

    // References of the player's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal PlayerMovementScript playerMovementScript;
    [SerializeField]
    internal PlayerInputScript playerInputScript;
    [SerializeField]
    internal PlayerAnimationScript playerAnimationScript;
    [SerializeField]
    internal Health healthScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main PlayerScript starting");

        // Set health
        healthScript.MaxHealth = health;
    }
}
