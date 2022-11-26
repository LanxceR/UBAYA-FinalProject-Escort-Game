using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AllyIsFacing { NORTH, EAST, SOUTH, WEST }

/// <summary>
/// The escortee ally animation script (handles all escortee ally model animations)
/// </summary>
/// 
public class AllyEscorteeAnimationScript : MonoBehaviour, IAnimation
{
    // Reference to the main enemy script
    [SerializeField]
    private AllyEscorteeScript allyEscorteeScript;

    // Components
    [SerializeField]
    private Animator animator;

    // Animation States
    public const string ALLY_IDLE_FRONT = "Idle Front";
    public const string ALLY_IDLE_RIGHT = "Idle Right";
    public const string ALLY_IDLE_BACK = "Idle Back";
    public const string ALLY_IDLE_LEFT = "Idle Left";

    public const string ALLY_DEATH = "Death";

    // Variables
    internal string currentState;
    private bool uninterruptibleCoroutineRunning = false;
    private string eAllyDir;

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
    private float GetFacingDirection()
    {
        float degAngle = 0f;
        if (allyEscorteeScript.seekTargetScript)
        {
            // If aiming at a target, get direction based off of that target position
            if (allyEscorteeScript.seekTargetScript.target) degAngle = Utilities.GetDirectionAngle(allyEscorteeScript.seekTargetScript.target.position - transform.position);
        }

        return degAngle;
    }

    private void UpdateAnimationDirection()
    {
        // TODO: Fix animation flickers from left/right for the first frame when transitioning (something to do with pathfinding direction flipping left/right momentarily).

        // Get Direction angle (right = 0 deg, anti-clockwise until 360 deg)
        float degAngle = GetFacingDirection();

        // Perform direction checking
        if (degAngle < 45 || 315 < degAngle)
        {
            eAllyDir = ALLY_IDLE_RIGHT;
        }
        else if (45 < degAngle && degAngle < 135)
        {
            // Facing back / up
            eAllyDir = ALLY_IDLE_BACK;
        }
        else if (135 < degAngle && degAngle < 225)
        {
            eAllyDir = ALLY_IDLE_LEFT;
        }
        else
        {
            eAllyDir = ALLY_IDLE_FRONT;
        }

        ChangeAnimationState(eAllyDir, false);
    }
    #endregion
}
