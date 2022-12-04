using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player movement script (handles all player movements)
/// </summary>
[RequireComponent(typeof(PlayerScript), typeof(MoveableScript))]
public class PlayerMovementScript : MonoBehaviour
{
    enum SpeedRecoveryMode { LINEAR, SMOOTHSTEP, SMOOTHSTEP2, EXPONENTIAL }

    // Reference to the main player script
    [SerializeField]
    private PlayerScript playerScript;

    // Components
    private MoveableScript moveableComp;

    // Variables
    [SerializeField]
    private float currentSpeed; // To store current calculated speed in respect to other possible modifiers
    [SerializeField]
    private float actualSpeed; // To store current actual speed instead of player's base speed
    [SerializeField]
    private SpeedRecoveryMode recoveryMode;
    internal Vector2 dir;
    private Coroutine knockbackRecoverCoroutine;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        Debug.Log("PlayerMovementScript starting");
        moveableComp = GetComponent<MoveableScript>();
        actualSpeed = playerScript.baseSpeed;
        currentSpeed = playerScript.baseSpeed;

        if (playerScript.knockbackScript)
        {
            // Add listener to Health's OnHit UnityEvent
            playerScript.knockbackScript.KnockbackRecovery.AddListener(KnockbackRecovery);
        }
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    // Move Player rigidbody in FixedUpdate
    private void FixedUpdate()
    {
        float MoveX = playerScript.playerInputScript.Input_MoveX;
        float MoveY = playerScript.playerInputScript.Input_MoveY;

        // Set direction vector for player movement
        dir = new Vector2(MoveX, MoveY);

        // Set moveable speed
        moveableComp.velocityThisFrame = actualSpeed;

        // Move using moveable
        moveableComp.SetDirection(dir);
    }

    // Method to recover from any knockbacks
    private void KnockbackRecovery(float recoveryTime)
    {
        if (knockbackRecoverCoroutine == null)
        {
            StartCoroutine(RecoverFromKnockback(recoveryTime));
        }
    }

    private IEnumerator RecoverFromKnockback(float recoveryTime)
    {
        float elapsedTime = 0f;

        // Set player's speed to 0
        actualSpeed = 0;

        // Gradually recover
        while (elapsedTime <= recoveryTime)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / recoveryTime;

            switch (recoveryMode)
            {
                case SpeedRecoveryMode.LINEAR:
                    // Linear
                    actualSpeed = Mathf.Lerp(0, currentSpeed, t);
                    break;
                case SpeedRecoveryMode.SMOOTHSTEP:
                    // SmoothStep
                    actualSpeed = Mathf.SmoothStep(0, currentSpeed, t);
                    break;
                case SpeedRecoveryMode.SMOOTHSTEP2:
                    // SmoothStep 2
                    t = t * t * (3f - 2f * t);
                    actualSpeed = Mathf.Lerp(0, currentSpeed, t);
                    break;
                case SpeedRecoveryMode.EXPONENTIAL:
                    // Exponential
                    t = t * t;
                    actualSpeed = Mathf.Lerp(0, currentSpeed, t);
                    break;
            }

            yield return null;
        }

        // Snap the actual speed to the current speed at the end
        actualSpeed = currentSpeed;

        knockbackRecoverCoroutine = null;
    }
}
