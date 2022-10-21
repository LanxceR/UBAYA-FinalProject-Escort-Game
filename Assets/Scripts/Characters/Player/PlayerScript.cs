using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main player script (or the hub)
/// </summary>
[RequireComponent(typeof(MoveableScript))]
public class PlayerScript : MonoBehaviour, ICharacter
{
    // Player stats
    [Header("Player Stats")]
    [SerializeField]
    internal float health = 5f;
    [SerializeField]
    internal float baseSpeed = 1f;
    [SerializeField]
    internal bool knockbackImmune = false;

    // References of the player's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal PlayerMovementScript playerMovementScript;
    [SerializeField]
    internal PlayerInputScript playerInputScript;
    [SerializeField]
    internal PlayerAnimationScript playerAnimationScript;
    [SerializeField]
    internal HealthScript healthScript;
    [SerializeField]
    internal KnockbackScript knockbackScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main PlayerScript starting");

        // Set health
        healthScript.MaxHealth = health;

        // Set knockback immunity
        knockbackScript.knockbackImmune = this.knockbackImmune;
    }
}
