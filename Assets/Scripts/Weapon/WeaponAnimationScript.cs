using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The weapon animation script (handles all weapon model animations)
/// </summary>
[RequireComponent(typeof(WeaponScript))]
public class WeaponAnimationScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    internal WeaponScript weaponScript;

    // Components
    [SerializeField]
    internal Animator animator;

    // Animation States
    public const string WEAPON_IDLE = "Weapon Idle";
    public const string WEAPON_ATTACK = "Weapon Attack";

    // Variables
    [SerializeField] private int frameToExecuteAttack;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WeaponAnimationScript starting");
    }

    // Update is called once per frame
    void Update()
    {
        // Always default to Idle after any animation has finished playing
        if (!AnimatorIsPlaying(WEAPON_IDLE) && AnimatorHasFinishedPlaying())
        {
            ChangeAnimationState(WEAPON_IDLE);
        }
    }

    // Method to change animation state
    internal void ChangeAnimationState(string newState)
    {
        // Prevent the same animation from interrupting itself
        if (AnimatorIsPlaying(newState)) return;

        // Play the animation
        animator.Play(newState);
    }

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
        weaponScript.weaponAttackScript.ShootProjectile();
    }

}
