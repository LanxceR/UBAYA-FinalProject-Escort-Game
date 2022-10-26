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
    [SerializeField] internal GameObject parentHolder;
    [SerializeField] internal GameObject parentAttach;

    [Header("Ammo")]
    [SerializeField] internal float startingAmmo = Mathf.Infinity;
    internal float Ammo { get; private set; }

    // TODO: Probably add a dedicated ammo manager sub-script?
    // References of the weapon's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal WeaponInputScript weaponInputScript;
    [SerializeField]
    internal WeaponSpriteScript weaponSpriteScript;
    [SerializeField]
    internal WeaponAnimationScript weaponAnimationScript;

    internal IAttackStrategy weaponAttackScript;

    // Variables
    internal float reloadElapsedTime;
    internal float reloadProgress;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main WeaponScript starting");

        weaponAttackScript = GetComponent<IAttackStrategy>();

        Ammo = startingAmmo;
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
        Transform t = this.transform;
        while (t.parent != null)
        {
            if (t.parent.TryGetComponent(out ICharacter _))
            {
                parentHolder = t.parent.gameObject; 
                return;
            }
            t = t.parent;
        }

        // Could not find a parent with implementing ICharacter
        parentHolder = this.gameObject;
    }

    private void AssignParentAttach()
    {
        parentAttach = Utilities.FindParentWithTag(gameObject, "WeaponAttachPos");
    }

    // To update ammo count
    internal void UpdateAmmo(float newAmmoCount)
    {
        Ammo = newAmmoCount;
    }
}
