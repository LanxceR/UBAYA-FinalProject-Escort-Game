using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The weapon animation script (handles all weapon model animations)
/// </summary>
[RequireComponent(typeof(WeaponScript))]
public class WeaponAnimationScript : MonoBehaviour, IAnimation
{
    // Reference to the main player script
    [SerializeField]
    private WeaponScript weaponScript;

    // Components
    [SerializeField]
    private Animator animator;

    // TODO: Add weapon reload anim
    // Animation States
    public const string WEAPON_IDLE = "Weapon Idle";
    public const string WEAPON_ATTACK = "Weapon Attack";

    // Settings
    [SerializeField] private int frameToExecuteAttack;

    // Variables
    private string currentState;
    private bool uninterruptibleCoroutineRunning = false;

    #region State machine
    // Update is called once per frame
    void Update()
    {
        // Always default to Idle after any animation has finished playing
        if ((currentState == WEAPON_ATTACK) && AnimatorHasFinishedPlaying())
        {
            ChangeAnimationState(WEAPON_IDLE, false);
        }
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
    // Coroutine to return to idle anim after the end of whatever anim clip is currently playing
    private IEnumerator ReturnToIdleCoroutine()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        ChangeAnimationState(WEAPON_IDLE, false);
    }

    // Coroutine to return to idle anim after the end of whatever anim clip is currently playing
    private IEnumerator ExitTransitionCoroutine()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        ChangeAnimationState(WEAPON_IDLE, false);
    }

    // Trigger attack anim
    internal void AttackAnimation()
    {
        ChangeAnimationState(WEAPON_ATTACK, false);

        // TODO: Implement shoot/attack timed on a specific frame on an animation clip
        // For now shooting / attacking (for melee) is handled through animation clips
        // Call Execute Attack method
        weaponScript.weaponAttackScript.ExecuteAttack();
    }
    #endregion
}
