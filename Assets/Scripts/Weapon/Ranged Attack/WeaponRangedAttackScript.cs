using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// The weapon ranged attack script (handles all weapon ranged attack action)
/// </summary>
[RequireComponent(typeof(WeaponScript), typeof(WeaponRangedMuzzleScript))]
public class WeaponRangedAttackScript : MonoBehaviour, IAttackStrategy
{
    // Reference to the main player script
    [SerializeField]
    private WeaponScript weaponScript;

    [SerializeField]
    private List<WeaponRangedMuzzleScript> weaponMuzzleScripts; //List of muzzleScripts for multiple shooting mechanics (alternate fires)
       
    // Weapon stats
    [Header("Weapon Stats")]
    // Main stats
    [SerializeField] internal float fireRateDelay = 0.1f;
    [SerializeField] internal float damage = 1f;
    [SerializeField] internal float range = 5f;
    [SerializeField] internal float velocity = 5f;
    [SerializeField] internal float knockbackForce = 10f;
    [Range(0, 359)] [SerializeField] internal float spread = 0f;
    [SerializeField] internal bool isFullAuto = false;

    // Burst fire
    [SerializeField] internal bool isBurstFire = false;
    [SerializeField] internal int burstAmount = 3;
    [SerializeField] internal float burstDelay = 0.03f;

    // Pierce
    [SerializeField] internal int pierceAmount = 0; // 0 means no piercing, 1 means pierces one enemy, etc
    [SerializeField] internal float pierceMultiplier = 0.5f; // Multiplier to apply to projectile after each pierced enemy
    [SerializeField] internal float minPierceMultiplier = 0.1f; // Min damage from pierce multiplier

    [Header("Misc Settings")]
    [SerializeField] private bool drawGizmo = false;

    // Variables
    private float cooldown = 0f;
    private bool canAttack = true;

    private WeaponID currentWeapon;
    private FMOD.Studio.EventInstance weaponAtk;

    void Start()
    {
        currentWeapon = weaponScript.id;
        weaponAtk = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Weapon/WeaponAttack");
        DetermineSound();
    }

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

        // Set canAttack to false for non full-auto weapons
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
    public void BeginAttack()
    {
        if (!canAttack) return;

        if (cooldown <= 0f && weaponScript.weaponAmmoScript.loadedAmmo > 0)
        {
            // Interrupt any ongoing reload
            weaponScript.weaponAmmoScript.InterruptReloadCoroutine();

            // If this weapon does NOT have an animation, fire/attack straight away
            // Otherwise call firing/attack in WeaponAnimation & animator
            if (!weaponScript.weaponAnimationScript)
                ExecuteAttack();
            else
                AttackWithAnim();

            weaponAtk.start();
        }
    }

    // Execute shoot anim (if available)
    public void AttackWithAnim()
    {
        weaponScript.weaponAnimationScript.AttackAnimation();
    }

    // Attempt to shoot projectile
    public void ExecuteAttack()
    {
        // Call a shoot projectile coroutine from each muzzle subscripts
        foreach (WeaponRangedMuzzleScript muzzleScript in weaponMuzzleScripts)
        {
            StartCoroutine(muzzleScript.ShootCoroutine());
        }

        // Set Cooldown
        cooldown = fireRateDelay;

        // Subtract ammo by one
        weaponScript.weaponAmmoScript.ChangeLoadedAmmo(-1);
    }


#if UNITY_EDITOR
    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        Vector3 textRootPos = transform.position + new Vector3(0.2f, 0.2f);
        int textPos = 0;

        // Weapon range (as a wireframe sphere)
        Gizmos.color = new Color(240f / 255, 120f / 255, 46f / 255);
        Gizmos.DrawWireSphere(transform.position, range);
    }
#endif

    void DetermineSound()
    {
        switch (currentWeapon)
        {
            case WeaponID.PISTOL:
                weaponAtk.setParameterByName("Weapon", 4);
                break;

            case WeaponID.SHOTGUN:
                weaponAtk.setParameterByName("Weapon", 5);
                break;

            case WeaponID.SMG:
                weaponAtk.setParameterByName("Weapon", 6);
                break;

            case WeaponID.RIFLE:
                weaponAtk.setParameterByName("Weapon", 7);
                break;
        }
    }
}
