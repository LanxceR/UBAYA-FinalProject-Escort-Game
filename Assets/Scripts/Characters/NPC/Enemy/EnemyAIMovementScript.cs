using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The enemy movement script (handles all player movements)
/// </summary>
[RequireComponent(typeof(EnemyScript), typeof(MoveableScript))]
public class EnemyAIMovementScript : MonoBehaviour
{    enum SpeedRecoveryMode { LINEAR, SMOOTHSTEP, SMOOTHSTEP2, EXPONENTIAL }

    // Reference to the main enemy script
    [SerializeField]
    private EnemyScript enemyScript;

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

    // Pathfinding Settings
    [Header("Pathfinding Settings")]
    [SerializeField] internal Transform target;
    [SerializeField] private bool followTarget = true;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Probably improve this assigning of target (maybe use property with custom setters?)
        if (GameManager.Instance.ActivePlayer.transform)
            target = GameManager.Instance.ActivePlayer.transform;
        enemyScript.pathfindingScript.target = target;

        moveableComp = GetComponent<MoveableScript>();
        actualSpeed = enemyScript.baseSpeed;
        currentSpeed = enemyScript.baseSpeed;

        if (enemyScript.knockbackScript)
        {
            // Add listener to Health's OnHit UnityEvent
            enemyScript.knockbackScript.KnockbackRecovery.AddListener(KnockbackRecovery);
        }
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        if (enemyScript.pathfindingScript)
            AIPathMovement();
    }

    private void AIPathMovement()
    {
        if (enemyScript.pathfindingScript.target)
        {
            if (followTarget)
            {
                // Get direction to the next waypoint in path (from pathfinder)
                dir = enemyScript.pathfindingScript.GetDirectionToFollowPath();

                // Set moveable speed
                moveableComp.speed = actualSpeed;

                // Move using moveable
                moveableComp.SetDirection(dir);
            }
            else
            {
                moveableComp.StopMoving();
            }
        }
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
