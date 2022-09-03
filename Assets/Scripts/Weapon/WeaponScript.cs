using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main weapon script (or the hub)
/// </summary>
public class WeaponScript : MonoBehaviour
{
    // Settings
    [Header("Parent Settings")]
    [SerializeField] internal GameObject parent;

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
    [SerializeField] internal int burstAmount = 3;
    [SerializeField] internal float burstDelay = 0.03f;

    [Header("Ammo")]
    [SerializeField] internal float startingAmmo = Mathf.Infinity;
    internal float Ammo { get; private set; }

    // References of the weapon's sub-scripts
    [SerializeField]
    internal WeaponInputScript weaponInputScript;
    [SerializeField]
    internal WeaponAnimationScript weaponAnimationScript;
    [SerializeField]
    internal WeaponAttackScript weaponAttackScript;
    [SerializeField]
    internal List<WeaponMuzzleScript> weaponMuzzleScripts; //List of muzzleScripts for multiple shooting mechanics
    [SerializeField]
    internal WeaponSpriteScript weaponSpriteScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main WeaponScript starting");
        Ammo = startingAmmo;
    }
}
