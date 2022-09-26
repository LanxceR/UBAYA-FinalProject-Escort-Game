using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    [SerializeField] internal float reloadTime = 1f;
    [SerializeField] internal int burstAmount = 3;
    [SerializeField] internal float burstDelay = 0.03f;

    [Header("Ammo")]
    [SerializeField] internal float startingAmmo = Mathf.Infinity;
    internal float Ammo { get; private set; }

    // TODO: Probably add a dedicated ammo manager sub-script?
    // References of the weapon's sub-scripts
    [Header("Sub-scripts")]
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

    [Header("Misc Settings")]
    [SerializeField] private bool drawGizmo = false;

    // Variables
    internal float reloadElapsedTime;
    internal float reloadProgress;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main WeaponScript starting");
        Ammo = startingAmmo;
    }

    // To update ammo count
    internal void UpdateAmmo(float newAmmoCount)
    {
        Ammo = newAmmoCount;
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
        Handles.Label(ammoTextPos, $"Ammo: {Ammo.ToString()}");
        textPos++;

        // Reloading (as text & completion percentage)
        Vector3 reloadTextPos = textRootPos + new Vector3(0f, 0.2f * textPos);
        if (reloadElapsedTime > 0)
        {
            Handles.Label(reloadTextPos, $"Reloading...{reloadProgress}%");
            textPos++;
        }
    }
#endif
}
