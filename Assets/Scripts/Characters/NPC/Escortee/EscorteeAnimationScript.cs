using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The escortee animation script (handles all escortee model animations)
/// </summary>
public class EscorteeAnimationScript : MonoBehaviour, IAnimation
{
    // Reference to the main player script
    [SerializeField]
    private EscorteeScript escorteeScript;

    // Components
    [SerializeField]
    private Animator animator;

    // Animation States
    public const string ESCORTEE_IDLE = "Idle";
    public const string ESCORTEE_MOVING = "Moving";
    public const string ESCORTEE_HURT = "Hurt";

    // Variables
    private string currentState;
    private bool uninterruptibleCoroutineRunning = false;

    #region Initialization
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("EscorteeAnimationScript starting");

        if (escorteeScript.healthScript)
        {
            // Add listener to Health's OnHit UnityEvent          
            escorteeScript.healthScript.OnHit?.AddListener(EscorteeHurt);
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
    #endregion

    #region Core transition function
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
    private void EscorteeHurt()
    {
        if (!uninterruptibleCoroutineRunning)
        {
            StartCoroutine(ChangeAnimationStateUninterruptible(ESCORTEE_HURT, false, false));
        }
    }

    // Update anim state based off of vehicle speed
    private void UpdateAnimationState()
    {
        // For abbreviation
        EscorteeMovementScript move = escorteeScript.escorteeMovementScript;

        // TODO: Maybe implement changing animator speed based off of speed
        if (move.speedStage == 0)
        {
            ChangeAnimationState(ESCORTEE_IDLE, false);
        }
        else
        {
            ChangeAnimationState(ESCORTEE_MOVING, false);
        }
    }
    #endregion
}
