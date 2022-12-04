using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The escortee input script (detects & store all escortee inputs)
/// </summary>
[RequireComponent(typeof(EscorteeScript))]
public class EscorteeInputScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private EscorteeScript escorteeScript;

    internal float Input_SlowDown { get; private set; }
    internal float Input_SpeedUp { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("EscorteeInputScript starting");
    }

    // OnESlowDown listener from InputAction "MainPlayerInput.inputaction"
    void OnESlowDown(InputValue value)
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        Input_SlowDown = value.Get<float>();
    }

    // OnESpeedUp listener from InputAction "MainPlayerInput.inputaction"
    void OnESpeedUp(InputValue value)
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        Input_SpeedUp = value.Get<float>();
    }
}
