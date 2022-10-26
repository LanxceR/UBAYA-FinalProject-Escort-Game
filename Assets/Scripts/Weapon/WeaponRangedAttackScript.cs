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
    private List<WeaponRangedMuzzleScript> weaponMuzzleScripts; //List of muzzleScripts for multiple shooting mechanics (shotgun, different firing modes)
       
    // Weapon stats
    [Header("Weapon Stats")]
    [SerializeField] internal float fireRateDelay = 0.1f;
    [SerializeField] internal float damage = 1f;
    [SerializeField] internal float range = 5f;
    [SerializeField] internal float velocity = 5f;
    [SerializeField] internal float knockbackForce = 10f;
    [Range(0, 359)] [SerializeField] internal float spread = 0f;
    [SerializeField] internal bool isFullAuto = false;
    [SerializeField] internal bool isBurstFire = false;
    [SerializeField] internal float reloadTime = 1f;
    [SerializeField] internal int burstAmount = 3;
    [SerializeField] internal float burstDelay = 0.03f;

    [Header("Misc Settings")]
    [SerializeField] private bool drawGizmo = false;

    // Variables
    private float cooldown = 0f;
    private bool canAttack = true;
    private Coroutine reloadCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WeaponRangedAttackScript starting");
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
                reloadCoroutine = StartCoroutine(ReloadCoroutine(reloadTime));
            }
        }

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

        if (weaponScript.Ammo <= 0 && reloadCoroutine == null)
        {
            // Reload
            reloadCoroutine = StartCoroutine(ReloadCoroutine(reloadTime));
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
                ExecuteAttack();
            else
                AttackWithAnim();
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
        weaponScript.UpdateAmmo(weaponScript.Ammo - 1);
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

        // TODO: Probably implement a better Text Gizmo Draw

        // Ammo count (as text)
        Vector3 ammoTextPos = textRootPos + new Vector3(0f, 0.2f * textPos);
        Handles.Label(ammoTextPos, $"Ammo: {weaponScript.Ammo.ToString()}");
        textPos++;

        // Reloading (as text & completion percentage)
        Vector3 reloadTextPos = textRootPos + new Vector3(0f, 0.2f * textPos);
        if (weaponScript.reloadElapsedTime > 0)
        {
            Handles.Label(reloadTextPos, $"Reloading...{weaponScript.reloadProgress}%");
            textPos++;
        }
    }
#endif
}
