using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyIsFacing { NORTH, EAST, SOUTH, WEST }

/// <summary>
/// The enemy animation script (handles all enemy model animations)
/// </summary>
/// 
[RequireComponent(typeof(EnemyScript))]
public class EnemyAnimationScript : MonoBehaviour, IAnimation
{
    // Reference to the main enemy script
    [SerializeField]
    private EnemyScript enemyScript;

    // Components
    [SerializeField]
    private Animator animator;

    // Animation States
    public const string ENEMY_IDLE_FRONT = "Idle Front";
    public const string ENEMY_IDLE_RIGHT = "Idle Right";
    public const string ENEMY_IDLE_BACK = "Idle Back";
    public const string ENEMY_IDLE_LEFT = "Idle Left";

    public const string ENEMY_RUN_FRONT = "Run Front";
    public const string ENEMY_RUN_RIGHT = "Run Right";
    public const string ENEMY_RUN_BACK = "Run Back";
    public const string ENEMY_RUN_LEFT = "Run Left";

    public const string ENEMY_HURT = "Hurt";
    public const string ENEMY_DEATH = "Death";

    public const string ENEMY_ATTACK_LEFT = "Attack Left";
    public const string ENEMY_ATTACK_RIGHT = "Attack Right";

    // Variables
    internal string currentState;
    private bool uninterruptibleCoroutineRunning = false;
    private string enemyDir;
    private Coroutine deathCoroutine;

    #region Initialization
    // Start is called before the first frame update
    void Start()
    {
        if (enemyScript.healthScript)
        {
            // Add listener to Health's OnHit UnityEvent         
            enemyScript.healthScript.OnHit?.AddListener(delegate { EnemyHurt(); });

            // Add listener to Health's OnHealthReachedZero UnityEvent          
            enemyScript.healthScript.OnHealthReachedZero?.AddListener(delegate { StartCoroutine(EnemyDeath()); });
        }
    }
    #endregion

    #region State machine
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        UpdateAnimationState();
    }

    // Central process to handle all anim state update
    void UpdateAnimationState()
    {
        // To double-check that death animation is played on dying
        if (enemyScript.healthScript)
        {
            if (enemyScript.healthScript.IsDead)
            {
                if (currentState != ENEMY_DEATH)
                    deathCoroutine = StartCoroutine(EnemyDeath());
            }
        }

        UpdateAnimationDirection();
    }
    #endregion

    #region Core transition functions
    // Method to change animation state
    public bool ChangeAnimationState(string newState, bool forceStart)
    {
        // If animator speed is 0, then return
        if (animator.speed == 0) return false;

        // Prevent the same animation from interrupting itself
        if (currentState == newState) return false;

        // If there's an uninterruptible animation currently running and is NOT forced to start an anim, return
        if (uninterruptibleCoroutineRunning && !forceStart) return false;

        // Play the animation
        animator.Play(newState);

        // Reassign the current state to the new one
        currentState = newState;

        return true;
    }

    // Method to change animation state to another state and make it uninterruptible
    public IEnumerator ChangeAnimationStateUninterruptible(string newState, bool forceStart, bool stopAfterAnimEnd)
    {
        // Anim transition
        if (!ChangeAnimationState(newState, forceStart))
        {
            yield break;
        }

        // Uses a bool to indicate if there's an uninterrupted anim running
        // NOTE: Using return value from StartCoroutine() sometimes doesn't work in this instance for some reason
        uninterruptibleCoroutineRunning = true;

        yield return new WaitForEndOfFrame();

        // Wait until played animations finishes
        while (!AnimatorHasFinishedPlaying(newState))
        {
            yield return null;
        }

        // Stop the animator
        if (stopAfterAnimEnd)
        {
            animator.speed = 0;
        }

        uninterruptibleCoroutineRunning = false;
    }
    #endregion

    #region State checking, other utilities
    // Method to check if there's any animation clip is currently playing
    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1;
    }

    // Method to check if currently playing anim clip has finished playing
    bool AnimatorHasFinishedPlaying()
    {
        return !AnimatorIsPlaying() && !animator.IsInTransition(0);
    }

    // Method to check if currently playing anim clip is a specific anim AND has finished playing
    bool AnimatorHasFinishedPlaying(string stateName)
    {
        return currentState == stateName && AnimatorHasFinishedPlaying();
    }
    #endregion

    #region Transitions
    // Play hurt animation
    private Coroutine EnemyHurt()
    {
        if (!uninterruptibleCoroutineRunning)
        {
            return StartCoroutine(ChangeAnimationStateUninterruptible(ENEMY_HURT, false, false));
        }

        return null;
    }

    // Play death animation
    private IEnumerator EnemyDeath()
    {
        yield return EnemyHurt();
        StartCoroutine(ChangeAnimationStateUninterruptible(ENEMY_DEATH, true, true));
    }

    private float GetFacingDirection()
    {
        float degAngle = 0f;
        if (enemyScript.recAggroScript)
        {
            // If aiming at a target, get direction based off of that target position
            if (enemyScript.recAggroScript.target) degAngle = Utilities.GetDirectionAngle(enemyScript.recAggroScript.target.position - transform.position);
            else degAngle = Utilities.GetDirectionAngle(enemyScript.enemyMovementScript.dir);
        }
        else
        {
            // Otherwise, get direction based off of movement
            degAngle = Utilities.GetDirectionAngle(enemyScript.enemyMovementScript.dir);
        }

        return degAngle;
    }

    // Trigger attack anim
    internal void AttackAnimation()
    {
        // Get Direction angle (right = 0 deg, anti-clockwise until 360 deg)
        float degAngle = GetFacingDirection();

        // Perform direction checking
        if (degAngle < 90 || 270 < degAngle)
        {
            enemyDir = ENEMY_ATTACK_RIGHT;
        }
        else
        {
            enemyDir = ENEMY_ATTACK_LEFT;
        }

        // TODO: Implement shoot/attack timed on a specific frame on an animation clip
        // For now shooting / attacking (for melee) is handled through animation clips
        StartCoroutine(ChangeAnimationStateUninterruptible(enemyDir, true, false));
    }

    private void UpdateAnimationDirection()
    {
        // TODO: Fix animation flickers from left/right for the first frame when transitioning (something to do with pathfinding direction flipping left/right momentarily).

        // Get Direction angle (right = 0 deg, anti-clockwise until 360 deg)
        float degAngle = GetFacingDirection();

        // Perform direction checking
        if (degAngle < 90 || 270 < degAngle)
        {
            // Facing right
            if (enemyScript.enemyMovementScript.dir.magnitude > 0)
                enemyDir = ENEMY_RUN_RIGHT;
            else
                enemyDir = ENEMY_IDLE_RIGHT;
        }
        else
        {
            // Facing left
            if (enemyScript.enemyMovementScript.dir.magnitude > 0)
                enemyDir = ENEMY_RUN_LEFT;
            else
                enemyDir = ENEMY_IDLE_LEFT;
        }

        ChangeAnimationState(enemyDir, false);
    } 
    #endregion
}
