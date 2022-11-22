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

    // OnPFire listener from InputAction "MainPlayerInput.inputaction"
    void OnPFire(InputValue value)
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        Input_Attack = value.Get<float>();
    }

    // OnPReload listener from InputAction "MainPlayerInput.inputaction"
    void OnPReload(InputValue value)
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        Input_Reload = value.Get<float>();
    }
}
