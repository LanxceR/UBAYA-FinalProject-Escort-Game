using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The player input script (detects & store all player inputs)
/// </summary>
[RequireComponent(typeof(PlayerScript))]
public class PlayerInputScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private PlayerScript playerScript;

    internal float Input_MoveX { get; private set; }
    internal float Input_MoveY { get; private set; }
    internal Vector2 Input_MousePosition { get; private set; }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        Debug.Log("PlayerInputScript starting");
    }

    // OnPMove listener from InputAction "MainPlayerInput.inputaction"
    // Sets MoveX and MoveY public properties
    void OnPMove(InputValue moveValue)
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        // Get input value
        Vector2 moveVector = moveValue.Get<Vector2>();

        // Value ranges between -1 and 1
        // -1    => left, down
        // 1     => right, up
        Input_MoveX = moveVector.x;
        Input_MoveY = moveVector.y;
    }

    // OnPLook listener from InputAction "MainPlayerInput.inputaction"
    void OnPLook(InputValue mousePos)
    {
        // Get mouse position on screen
        Input_MousePosition = mousePos.Get<Vector2>();
    }
}
