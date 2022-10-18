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
    private WeaponScript weaponScript;

    // Variables
    private float cooldown = 0f;
    private bool canAttack = true;
    private Coroutine reloadCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WeaponAttackScript starting");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameIsPlaying) return;
        
        // Countdown cooldown until zero
        cooldown = cooldown - Time.deltaTime > 0 ? cooldown - Time.deltaTime : 0f;

        if (weaponScript.weaponInputScript.Input_Reload == 1)
        {
            if (weaponScript.Ammo < weaponScript.startingAmmo && reloadCoroutine == null)
            {
                // Reload
                reloadCoroutine = StartCoroutine(ReloadCoroutine(weaponScript.reloadTime));
            }
        }

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

        if (weaponScript.Ammo <= 0 && reloadCoroutine == null)
        {
            // Reload
            reloadCoroutine = StartCoroutine(ReloadCoroutine(weaponScript.reloadTime));
        }
        else if (cooldown <= 0f && weaponScript.Ammo > 0)
        {
            // Interrupt any ongoing reload
            if (reloadCoroutine != null)
            {
                StopCoroutine(reloadCoroutine);
                reloadCoroutine = null;
                weaponScript.reloadElapsedTime = 0f;
                weaponScript.reloadProgress = 0f;
            }

            // If this weapon does NOT have an animation, fire/attack straight away
            // Otherwise call firing/attack in WeaponAnimation & animator
            if (!weaponScript.weaponAnimationScript)
                ShootProjectile();
            else
                AttackWithAnim();
        }
    }

    // Execute shoot anim (if available)
    internal void AttackWithAnim()
    {
        weaponScript.weaponAnimationScript.AttackAnimation();
    }

    // Weapon reload
    private IEnumerator ReloadCoroutine(float reloadTime)
    {
        weaponScript.reloadElapsedTime = 0f;
        weaponScript.reloadProgress = 0f;

        while (weaponScript.reloadElapsedTime < reloadTime)
        {
            weaponScript.reloadElapsedTime += Time.deltaTime;
            weaponScript.reloadProgress = (weaponScript.reloadElapsedTime / reloadTime * 100);

            yield return null;
        }

        // Reload weapon's ammo
        weaponScript.UpdateAmmo(weaponScript.startingAmmo);

        weaponScript.reloadElapsedTime = 0f;
        weaponScript.reloadProgress = 0f;

        reloadCoroutine = null;
    }

    // Attempt to shoot projectile
    internal void ShootProjectile()
    {
        // Call a shoot projectile coroutine from each muzzle subscripts
        foreach (WeaponMuzzleScript muzzleScript in weaponScript.weaponMuzzleScripts)
        {
            StartCoroutine(muzzleScript.ShootCoroutine());
        }

        // Set Cooldown
        cooldown = weaponScript.fireRateDelay;

        // Subtract ammo by one
        weaponScript.UpdateAmmo(weaponScript.Ammo - 1);
    }
}
