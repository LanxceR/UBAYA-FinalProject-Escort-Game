using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyIsFacing { NORTH, EAST, SOUTH, WEST }

/// <summary>
/// The enemy animation script (handles all enemy model animations)
/// </summary>
/// 
[RequireComponent(typeof(EnemyScript))]
public class EnemyAnimationScript : MonoBehaviour
{
    // Reference to the main enemy script
    [SerializeField]
    private EnemyScript enemyScript;

    // Components
    [SerializeField]
    private Animator animator;

    // TODO: Death animation
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

    // Variables
    private string currentState;
    private bool uninterruptibleCoroutineRunning = false;
    private string enemyDir;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("EnemyAnimationScript starting");

        if (enemyScript.healthScript)
        {
            // Add listener to Health's OnHit UnityEvent
            //UnityEditor.Events.UnityEventTools.AddPersistentListener(enemyScript.healthScript.OnHit, EnemyHurt);            
            enemyScript.healthScript.OnHit?.AddListener(EnemyHurt);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        UpdateAnimationState();
    }

    // Method to change animation state
    internal void ChangeAnimationState(string newState)
    {
        // Prevent the same animation from interrupting itself
        if (currentState == newState) return;

        // If there's an uninterruptible animation currently running, return
        if (uninterruptibleCoroutineRunning) return;

        // Play the animation
        animator.Play(newState);

        // Reassign the current state to the new one
        currentState = newState;
    }

    private IEnumerator ChangeAnimationStateUninterruptible(string newState)
    {
        // Anim transition
        ChangeAnimationState(newState);

        // Uses a bool to indicate if there's an uninterrupted anim running
        // NOTE: Using return value from StartCoroutine() sometimes doesn't work in this instance for some reason
        uninterruptibleCoroutineRunning = true;

        // Wait until hurt animations finishes
        while (!AnimatorHasFinishedPlaying(newState))
        {
            yield return null;
        }

        uninterruptibleCoroutineRunning = false;
    }


    // Method to check if there's any animation clip is currently playing
    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }

    // Method to check if an animation clip is currently playing
    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    // Method to check if currently playing anim clip has finished playing
    bool AnimatorHasFinishedPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0);
    }

    // Method to check if currently playing anim clip is a specific anim AND has finished playing
    bool AnimatorHasFinishedPlaying(string stateName)
    {
        return AnimatorIsPlaying(stateName) && AnimatorHasFinishedPlaying();
    }

    // Central process to handle all anim state update
    void UpdateAnimationState()
    {
        UpdateAnimationDirection();
    }

    // Play hurt animation
    private void EnemyHurt()
    {
        if (!uninterruptibleCoroutineRunning)
        {
            StartCoroutine(ChangeAnimationStateUninterruptible(ENEMY_HURT));
        }
    }

    private void UpdateAnimationDirection()
    {
        // TODO: Update enemy sprite facing based off of movement
        ChangeAnimationState(ENEMY_IDLE_FRONT);
    }
}
