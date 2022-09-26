using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The weapon input script (detects & store all weapon inputs)
/// </summary>
[RequireComponent(typeof(WeaponScript))]
public class WeaponInputScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    internal WeaponScript weaponScript;

    // 1 = performed, 0 = canceled
    internal float Input_Attack { get; private set; }
    internal float Input_Reload { get; private set; }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        Debug.Log("WeaponInputScript starting");
    }

    // OnPFire listener from InputAction "MainPlayerInput.inputaction"
    void OnPFire(InputValue value)
    {
        Input_Attack = value.Get<float>();
    }

    // OnPReload listener from InputAction "MainPlayerInput.inputaction"
    void OnPReload(InputValue value)
    {
        Input_Reload = value.Get<float>();
        Debug.Log(Input_Reload);
    }
}
