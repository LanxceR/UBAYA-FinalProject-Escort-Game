using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheatsManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;

    [SerializeField]
    internal bool playerGodMode;
    [SerializeField]
    internal bool escorteeGodMode;

    // Start is called before the first frame update
    void Start()
    {
        if (!gameManager)
        {
            // If Game Manager is not assigned in inspector, try and get one
            gameManager = Utilities.FindParentOfType<GameManager>(transform, out _);
        }
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        // Deactivate player god mode when there's no longer an active player
        if (!gameManager.gamePlayer.ActivePlayer && playerGodMode)
        {
            Debug.Log("PLAYER GOD MODE DEACTIVATED");
            playerGodMode = false;
        }
        // Deactivate escortee god mode when there's no longer an active player
        if (!gameManager.gameEscortee.ActiveEscortee && escorteeGodMode)
        {
            Debug.Log("ESCORTEE GOD MODE DEACTIVATED");
            escorteeGodMode = false;
        }
    }

    // OnPlayerGodMode listener from InputAction "MainPlayerInput.inputaction"
    // Mapped in Cheats
    void OnPlayerGodMode(InputValue val)
    {
        if (gameManager)
        {
            // Activate player god mode when there's an active player
            if (!playerGodMode && gameManager.gamePlayer.ActivePlayer)
            {
                SetPlayerGodMode(true);
            }
            // Deactivate player god mode when there's an active player
            else if (playerGodMode && gameManager.gamePlayer.ActivePlayer)
            {
                SetPlayerGodMode(false);
            }
        }
    }    
    // Activate/Deactivate player god mode ONLY when there's an active player
    private void SetPlayerGodMode(bool value)
    {
        if (!gameManager.gamePlayer.ActivePlayer) return;

        if (value)
            Debug.Log("PLAYER GOD MODE ACTIVATED");
        else
            Debug.Log("PLAYER GOD MODE DEACTIVATED");

        playerGodMode = value;
        gameManager.gamePlayer.ActivePlayer.healthScript.IsInvincible = value;
    }


    // OnEscorteeGodMode listener from InputAction "MainPlayerInput.inputaction"
    // Mapped in Cheats
    void OnEscorteeGodMode(InputValue val)
    {
        if (gameManager)
        {
            // Activate escortee god mode when there's an active player
            if (!escorteeGodMode && gameManager.gameEscortee.ActiveEscortee)
            {
                SetEscorteeGodMode(true);
            }
            // Deactivate escortee god mode when there's an active player
            else if (escorteeGodMode && gameManager.gameEscortee.ActiveEscortee)
            {
                SetEscorteeGodMode(false);
            }
        }
    }
    // Activate/Deactivate escortee god mode ONLY when there's an active player
    private void SetEscorteeGodMode(bool value)
    {
        if (!gameManager.gameEscortee.ActiveEscortee) return;

        if (value)
            Debug.Log("ESCORTEE GOD MODE ACTIVATED");
        else
            Debug.Log("ESCORTEE GOD MODE DEACTIVATED");

        escorteeGodMode = value;
        gameManager.gameEscortee.ActiveEscortee.healthScript.IsInvincible = value;
    }
}
