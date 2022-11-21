using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum WeaponID
{
    NONE,
    MELEE_INVISIBILE,
    RANGED_INVISIBLE,
    PIPE,
    KNIFE,
    BASEBALL_BAT,
    MACHETE,
    PISTOL,
    SHOTGUN,
    SMG,
    RIFLE
}

public enum AmmoType { NONE, LIGHT, SHOTGUN, HEAVY }

/// <summary>
/// The main weapon script (or the hub)
/// </summary>
public class WeaponScript : MonoBehaviour, IEquipmentItem
{
    // Weapon type
    [Header("Weapon ID")]
    [SerializeField]
    internal WeaponID id;

    // Settings
    [Header("Parent Settings")]
    [SerializeField] internal GameObject parentHolder;
    [SerializeField] internal GameObject parentAttach;

    [Header("Ammo")]
    [SerializeField] internal float reloadTime = 1f;
    [SerializeField] internal AmmoType ammoType;
    [SerializeField] internal float ammoMagSize = Mathf.Infinity;

    [Header("Price")]
    [SerializeField] internal float price = 1000f;

    [Header("Flags")]
    [SerializeField] internal bool isOwned;
    [SerializeField] internal bool isEquipped;

    // References of the weapon's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal WeaponInputScript weaponInputScript;
    [SerializeField]
    internal WeaponSpriteScript weaponSpriteScript;
    [SerializeField]
    internal WeaponAnimationScript weaponAnimationScript;
    [SerializeField]
    internal WeaponAmmoScript weaponAmmoScript;

    internal IAttackStrategy weaponAttackScript;

    // Variables
    internal float reloadElapsedTime = 0f;
    internal float reloadProgress = 0f;

    // Start is called before the first frame update
    void Start()
    {
        AssignComponents();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        AssignParentHolder();
        AssignParentAttach();
    }

    internal void AssignComponents()
    {
        weaponAttackScript = GetComponent<IAttackStrategy>();
    }

    // Try to assign a valid parent that implements the ICharacter interface
    private void AssignParentHolder()
    {
        parentHolder = Utilities.FindParent<ICharacter>(transform, out _).gameObject;
    }

    private void AssignParentAttach()
    {
        parentAttach = Utilities.FindParentWithTag(gameObject, "WeaponAttachPos", out _);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
