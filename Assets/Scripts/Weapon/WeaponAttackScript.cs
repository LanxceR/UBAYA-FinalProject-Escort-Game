using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The weapon attack script (handles all weapon attack action)
/// </summary>
public class WeaponAttackScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    internal WeaponScript weaponScript;

    // Variables
    private float cooldown = 0f;
    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WeaponAttackScript starting");
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown cooldown until zero
        cooldown = cooldown - Time.deltaTime > 0 ? cooldown - Time.deltaTime : 0f;

        if (weaponScript.weaponInputScript.Input_Attack == 1)
        {
            if (weaponScript.isFullAuto)
            {
                // If full auto, attack continously
                Attack();
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
        Attack();

        // Set canAttack to false non full-auto weapons
        canAttack = false;

        while (weaponScript.weaponInputScript.Input_Attack == 1)
        {
            // While attack button is still performed, wait until the next frame
            yield return null;
        }

        // After attack button is released/canceled, reset canAttack to true
        canAttack = true;
    }

    // Weapon attack
    internal void Attack()
    {
        if (!canAttack) return;

        // If this weapon does NOT have an animation, fire/attack straight away
        // Otherwise call firing/attack in WeaponAnimation & animator
        if (!weaponScript.weaponAnimationScript)
            ShootProjectile();
        else
            AttackWithAnim();
    }

    // Execute shoot anim (if available)
    internal void AttackWithAnim()
    {
        if (cooldown <= 0f && weaponScript.Ammo > 0)
        {
            weaponScript.weaponAnimationScript.AttackAnimation();
        }
    }

    // Attempt to shoot projectile
    internal void ShootProjectile()
    {
        if (cooldown <= 0f && weaponScript.Ammo > 0)
        {
            // Call a shoot projectile coroutine from each muzzle subscripts
            foreach (WeaponMuzzleScript muzzleScript in weaponScript.weaponMuzzleScripts)
            {
                StartCoroutine(muzzleScript.ShootCoroutine());
            }

            // Set Cooldown
            cooldown = weaponScript.fireRateDelay;
        }
    }
}
