using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// The main weapon script (or the hub)
/// </summary>
public enum AmmoType { NONE, LIGHT, SHOTGUN, HEAVY }
public class WeaponScript : MonoBehaviour, IEquipmentItem
{
    // Settings
    [Header("Parent Settings")]
    [SerializeField] internal GameObject parentHolder;
    [SerializeField] internal GameObject parentAttach;

    [Header("Ammo")]
    [SerializeField] internal float reloadTime = 1f;
    [SerializeField] internal AmmoType ammoType;
    [SerializeField] internal float ammoMagSize = Mathf.Infinity;

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
        Debug.Log("Main WeaponScript starting");

        weaponAttackScript = GetComponent<IAttackStrategy>();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        AssignParentHolder();
        AssignParentAttach();
    }

    // Try to assign a valid parent that implements the ICharacter interface
    private void AssignParentHolder()
    {
        parentHolder = FindICharacterParent().gameObject;
    }

    private void AssignParentAttach()
    {
        parentAttach = Utilities.FindParentWithTag(gameObject, "WeaponAttachPos");
    }

    private Transform FindICharacterParent()
    {
        Transform t = this.transform;
        while (t.parent != null)
        {
            if (t.parent.TryGetComponent(out ICharacter _))
            {
                return t.parent;
            }
            t = t.parent;
        }
        
        // Could not find a parent with implementing ICharacter
        return this.transform;
    }
}
