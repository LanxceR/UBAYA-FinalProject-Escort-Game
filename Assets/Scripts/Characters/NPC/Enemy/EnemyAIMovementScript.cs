using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The enemy movement script (handles all enemy movements)
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
    [SerializeField] private Transform target;
    internal Transform Target { get => target; set 
        {
            target = value;
            enemyScript.pathfindingScript.Target = value;
        } 
    }
    [SerializeField] private bool followTarget = true;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        moveableComp = GetComponent<MoveableScript>();

        actualSpeed = enemyScript.baseSpeed;
        currentSpeed = enemyScript.baseSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetAITarget();

        if (enemyScript.knockbackScript)
        {
            // Add listener to Knockback's OnHit UnityEvent
            enemyScript.knockbackScript.KnockbackRecovery.AddListener(KnockbackRecovery);
        }
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        // Check the current health state (dead or not) if it has one
        if (enemyScript.healthScript)
        {
            if (enemyScript.healthScript.IsDead == true)
                moveableComp.enabled = false;
        }

        // If attacking, stop moving until finished attacking
        if (enemyScript.enemyAnimationScript.currentState == EnemyAnimationScript.ENEMY_ATTACK_LEFT ||
            enemyScript.enemyAnimationScript.currentState == EnemyAnimationScript.ENEMY_ATTACK_RIGHT)
        {
            followTarget = false;
        }
        else
        {
            followTarget = true;
        }

        // Assign target for AI
        SetAITarget();

        // AI Movement
        if (enemyScript.pathfindingScript)
            AIPathMovement();
    }

    internal void SetDirection(Vector2 dir)
    {
        this.dir = dir;
        moveableComp.velocityThisFrame = actualSpeed;
        moveableComp.SetDirection(this.dir);
    }

    private void SetAITarget()
    {
        // Assign target for AI
        if (enemyScript.recAggroScript)
            Target = enemyScript.recAggroScript.target;
        else if (Target == null && GameManager.Instance.gamePlayer.ActivePlayer)
            Target = GameManager.Instance.gamePlayer.ActivePlayer.transform;
    }

    private void AIPathMovement()
    {
        if (enemyScript.pathfindingScript.Target)
        {
            if (followTarget)
            {
                // Get direction to the next waypoint in path (from pathfinder)
                dir = enemyScript.pathfindingScript.GetDirectionToFollowPath();

                // Set moveable speed
                moveableComp.velocityThisFrame = actualSpeed;

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
