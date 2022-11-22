using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// The weapon melee attack script (handles all weapon melee attack action)
/// </summary>
[RequireComponent(typeof(WeaponScript), typeof(WeaponMeleeHitScript), typeof(CollisionScript))]
[RequireComponent(typeof(Rigidbody2D))]
public class WeaponMeleeAttackScript : MonoBehaviour, IAttackStrategy
{
    // Reference to the main player script
    [SerializeField]
    private WeaponScript weaponScript;

    [SerializeField]
    private WeaponMeleeHitScript weaponMeleeHitScript;

    // Weapon stats
    [Header("Weapon Stats")]
    [SerializeField] internal float attackDelay = 1f;
    [SerializeField] internal float damage = 1f;
    //[SerializeField] internal float range = 5f;
    [SerializeField] internal float knockbackForce = 10f;
    [SerializeField] internal bool isFullAuto = false;

    [Header("Misc Settings")]
    [SerializeField] private bool drawGizmo = false;

    // Variables
    private float cooldown = 0f;
    private bool canAttack = true;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        // Countdown cooldown until zero
        cooldown = cooldown - Time.deltaTime > 0 ? cooldown - Time.deltaTime : 0f;

        if (weaponScript.weaponInputScript.Input_Attack == 1)
        {
            if (isFullAuto)
            {
                // If full auto, attack continously
                BeginAttack();
            }
            else
            {
                // If not, start coroutine for non-auto attack
                StartCoroutine(SemiAutoAttackCoroutine());
            }
        }
    }

    private IEnumerator SemiAutoAttackCoroutine()
    {
        BeginAttack();

        // Set canAttack to false non full-auto weapons
        canAttack = false;

        while (cooldown <= 0f && weaponScript.weaponInputScript.Input_Attack == 1)
        {
            // While attack button is still performed, wait until the next frame
            yield return null;
        }

        // After attack button is released/canceled, reset canAttack to true
        canAttack = true;
    }

    public void BeginAttack()
    {
        if (!canAttack) return;
        
        if (cooldown <= 0f && weaponScript.weaponAmmoScript.loadedAmmo > 0)
        {
            // If this weapon does NOT have an animation, fire/attack straight away
            // Otherwise call firing/attack in WeaponAnimation & animator
            if (!weaponScript.weaponAnimationScript)
                ExecuteAttack();
            else
                AttackWithAnim(); // AttackWithAnim => AttackAnimation => ExecuteAttack
        }
    }


    public void AttackWithAnim()
    {
        weaponScript.weaponAnimationScript.AttackAnimation();
    }

    public void ExecuteAttack()
    {
        // Set attacker
        weaponMeleeHitScript.SetAttacker(weaponScript.parentHolder);

        // Set Cooldown
        cooldown = attackDelay;

        // Enabling and disabling colliders are handled in animation clip
        return;
    }
}
