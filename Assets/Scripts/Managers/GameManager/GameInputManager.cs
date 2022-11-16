using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The GLOBAL (and general) game input manager script
/// </summary>
public class GameInputManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;

    // OnCancel listener from InputAction "MainPlayerInput.inputaction"
    // Mapped in UI
    void OnCancel(InputValue moveValue)
    {
        // If gameState is enabled, pause/resume the game
        if (gameManager.gameState.isActiveAndEnabled)
            gameManager.gameState.PauseAndResumeGame();
    }
}
