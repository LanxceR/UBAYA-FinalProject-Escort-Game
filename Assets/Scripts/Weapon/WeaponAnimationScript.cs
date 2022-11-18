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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WeaponAnimationScript starting");
    }

    #region State machine
    // Update is called once per frame
    void Update()
    {
        // Always default to Idle after any animation has finished playing
        if (!AnimatorIsPlaying(WEAPON_IDLE) && AnimatorHasFinishedPlaying())
        {
            ChangeAnimationState(WEAPON_IDLE);
        }
    }
    #endregion

    #region Core transition functions
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
    public IEnumerator ChangeAnimationStateUninterruptible(string newState, bool forceStart, bool stopAfterAnimEnd)
    {
        // If forced to start, then change uninterruptibleCoroutine flag to false
        if (forceStart) uninterruptibleCoroutineRunning = false;

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
    // Coroutine to return to idle anim after the end of whatever anim clip is currently playing
    private IEnumerator ReturnToIdleCoroutine()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        ChangeAnimationState(WEAPON_IDLE);
    }

    // Coroutine to return to idle anim after the end of whatever anim clip is currently playing
    private IEnumerator ExitTransitionCoroutine()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        ChangeAnimationState(WEAPON_IDLE);
    }

    // Trigger attack anim
    internal void AttackAnimation()
    {
        ChangeAnimationState(WEAPON_ATTACK);

        // TODO: Implement shoot/attack timed on a specific frame on an animation clip
        // For now shooting / attacking (for melee) is handled through animation clips
        // Call Execute Attack method
        weaponScript.weaponAttackScript.ExecuteAttack();
    }
    #endregion
}
