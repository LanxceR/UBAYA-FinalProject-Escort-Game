using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The projectile animation script (handles all projectile model animations)
/// </summary>
public class ProjectileAnimationScript : MonoBehaviour
{
    // Reference to the main projectile script
    [SerializeField]
    private ProjectileScript projectileScript;

    // Components
    [SerializeField]
    private Animator animator;
    [SerializeField]
    internal SpriteRenderer model;
}
