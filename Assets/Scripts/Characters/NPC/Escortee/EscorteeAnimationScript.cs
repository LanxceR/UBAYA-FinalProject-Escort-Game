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
            escorteeScript.healthScript.OnHit.AddListener(EscorteeHurt);
        }
    } 
    #endregion

    #region State machine
    // Update is called once per frame
    void Update()
    {
        UpdateAnimationState();
    } 
    #endregion

    #region Core transition function
    // Method to change animation state
    public void ChangeAnimationState(string newState)
    {
        // If animator speed is 0, then return
        if (animator.speed == 0) return;

        // Prevent the same animation from interrupting itself
        if (AnimatorIsPlaying(newState)) return;

        // If there's an uninterruptible animation currently running, return
        if (uninterruptibleCoroutineRunning) return;

        // Play the animation
        animator.Play(newState);

        // Reassign the current state to the new one
        currentState = newState;
    }

    // Method to change animation state to another state and make it uninterruptible
    public IEnumerator ChangeAnimationStateUninterruptible(string newState, bool stopAfterAnimEnd)
    {
        // Anim transition
        ChangeAnimationState(newState);

        // Uses a bool to indicate if there's an uninterrupted anim running
        // NOTE: Using return value from StartCoroutine() sometimes doesn't work in this instance for some reason
        uninterruptibleCoroutineRunning = true;

        // Wait until played animations finishes
        while (!AnimatorHasFinishedPlaying(newState))
        {
            yield return null;
        }

        // Stop the animator
        if (stopAfterAnimEnd) animator.speed = 0;

        uninterruptibleCoroutineRunning = false;
    }
    #endregion

    #region State checking, other utilities
    // Method to check if there's any animation clip is currently playing
    bool AnimatorIsPlaying()
    {
        // Mod'd by 1
        // This is because the integer part is the number of time a state has been looped
        return (animator.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 1;
    }

    // Method to check if an animation clip is currently playing
    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    // Method to check if currently playing anim clip has finished playing
    bool AnimatorHasFinishedPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animator.IsInTransition(0);
    }

    // Method to check if currently playing anim clip is a specific anim AND has finished playing
    bool AnimatorHasFinishedPlaying(string stateName)
    {
        return AnimatorIsPlaying(stateName) && AnimatorHasFinishedPlaying();
    }
    #endregion

    #region Transitions
    // Play hurt animation
    private void EscorteeHurt()
    {
        if (!uninterruptibleCoroutineRunning)
        {
            StartCoroutine(ChangeAnimationStateUninterruptible(ESCORTEE_HURT, false));
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
            ChangeAnimationState(ESCORTEE_IDLE);
        }
        else
        {
            ChangeAnimationState(ESCORTEE_MOVING);
        }
    }
    #endregion
}
