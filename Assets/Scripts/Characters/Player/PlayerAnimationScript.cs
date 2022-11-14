using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum PlayerIsFacing {NORTH, EAST, SOUTH, WEST }

/// <summary>
/// The player animation script (handles all player model animations)
/// </summary>
/// 
[RequireComponent(typeof(PlayerScript))]
public class PlayerAnimationScript : MonoBehaviour, IAnimation
{
    // Reference to the main player script
    [SerializeField]
    private PlayerScript playerScript;

    // Components
    [SerializeField]
    private Animator animator;

    // Animation States
    public const string PLAYER_IDLE_FRONT = "Player Idle Front";
    public const string PLAYER_IDLE_RIGHT = "Player Idle Right";
    public const string PLAYER_IDLE_BACK = "Player Idle Back";
    public const string PLAYER_IDLE_LEFT = "Player Idle Left";

    public const string PLAYER_RUN_FRONT = "Player Run Front";
    public const string PLAYER_RUN_RIGHT = "Player Run Right";
    public const string PLAYER_RUN_BACK = "Player Run Back";
    public const string PLAYER_RUN_LEFT = "Player Run Left";

    public const string PLAYER_HURT = "Player Hurt";
    public const string PLAYER_DEATH = "Player Death";

    // Variables
    private string currentState;
    private bool uninterruptibleCoroutineRunning = false;
    private string playerDir;

    #region Initialization
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerAnimationScript starting");

        if (playerScript.healthScript)
        {
            // Add listener to Health's OnHit UnityEvent          
            playerScript.healthScript.OnHit.AddListener(PlayerHurt);

            // Add listener to Health's OnHealthReachedZero UnityEvent          
            playerScript.healthScript.OnHealthReachedZero.AddListener(PlayerDeath);
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
        UpdateAnimationDirection();
    }
    #endregion

    #region Core transition functions
    // Method to change animation state
    public void ChangeAnimationState(string newState)
    {
        // If animator speed is 0, then return
        if (animator.speed == 0) return;

        // Prevent the same animation from interrupting itself
        if (currentState == newState) return;

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
    #endregion

    #region Transitions
    // Play hurt animation
    private void PlayerHurt()
    {
        if (!uninterruptibleCoroutineRunning)
        {
            StartCoroutine(ChangeAnimationStateUninterruptible(PLAYER_HURT, false));
        }
    }

    // Play death animation
    private void PlayerDeath()
    {
        if (!uninterruptibleCoroutineRunning)
        {
            StartCoroutine(ChangeAnimationStateUninterruptible(PLAYER_DEATH, true));
        }
    }

    private float GetPlayerFacingDirection()
    {
        // Get mouse position on screen
        Vector2 mousePos = playerScript.playerInputScript.Input_MousePosition;

        // Translate screen position to world position
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Get Angle
        // Get the vector
        Vector3 direction = (Vector3)worldMousePos - transform.position;
        // Get the angle in degrees (-180, 180)
        float degAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Normalize angle (0, 360)
        if (degAngle < 0) degAngle += 360;

        return degAngle;
    }

    private void UpdateAnimationDirection()
    {
        if (playerScript.playerInputScript)
        {
            // Get Player Direction angle (right = 0 deg, anti-clockwise until 360 deg)
            float degAngle = GetPlayerFacingDirection();

            // Perform direction checking
            if (degAngle < 45 || 315 < degAngle)
            {
                // Facing right
                if (playerScript.playerMovementScript.dir.magnitude > 0)
                    playerDir = PLAYER_RUN_RIGHT;
                else
                    playerDir = PLAYER_IDLE_RIGHT;
            }
            else if (45 < degAngle && degAngle < 135)
            {
                // Facing back / up
                if (playerScript.playerMovementScript.dir.magnitude > 0)
                    playerDir = PLAYER_RUN_BACK;
                else
                    playerDir = PLAYER_IDLE_BACK;
            }
            else if (135 < degAngle && degAngle < 225)
            {
                // Facing left
                if (playerScript.playerMovementScript.dir.magnitude > 0)
                    playerDir = PLAYER_RUN_LEFT;
                else
                    playerDir = PLAYER_IDLE_LEFT;
            }
            else
            {
                // Facing front / down
                if (playerScript.playerMovementScript.dir.magnitude > 0)
                    playerDir = PLAYER_RUN_FRONT;
                else
                    playerDir = PLAYER_IDLE_FRONT;
            }

            ChangeAnimationState(playerDir);
        }
        else
        {
            // Get Direction angle (right = 0 deg, anti-clockwise until 360 deg)
            float degAngle = Utilities.GetDirectionAngle(playerScript.playerMovementScript.dir);

            // Perform direction checking
            if (degAngle < 45 || 315 < degAngle)
            {
                // Facing right
                if (playerScript.playerMovementScript.dir.magnitude > 0)
                    playerDir = PLAYER_RUN_RIGHT;
                else
                    playerDir = PLAYER_IDLE_RIGHT;
            }
            else if (45 < degAngle && degAngle < 135)
            {
                // Facing back / up
                if (playerScript.playerMovementScript.dir.magnitude > 0)
                    playerDir = PLAYER_RUN_BACK;
                else
                    playerDir = PLAYER_IDLE_BACK;
            }
            else if (135 < degAngle && degAngle < 225)
            {
                // Facing left
                if (playerScript.playerMovementScript.dir.magnitude > 0)
                    playerDir = PLAYER_RUN_LEFT;
                else
                    playerDir = PLAYER_IDLE_LEFT;
            }
            else
            {
                // Facing front / down
                if (playerScript.playerMovementScript.dir.magnitude > 0)
                    playerDir = PLAYER_RUN_FRONT;
                else
                    playerDir = PLAYER_IDLE_FRONT;
            }

            ChangeAnimationState(playerDir);
        }
    }
    #endregion
}
