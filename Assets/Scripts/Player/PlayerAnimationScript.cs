using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum PlayerIsFacing {NORTH, EAST, SOUTH, WEST }

/// <summary>
/// The player animation script (handles all player model animations)
/// </summary>
/// 
[RequireComponent(typeof(PlayerScript))]
public class PlayerAnimationScript : MonoBehaviour
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

    // Variables
    private string currentState;
    private string playerDir;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerAnimationScript starting");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        UpdateAnimationDirection();
    }

    // Method to change animation state
    internal void ChangeAnimationState(string newState)
    {
        // Prevent the same animation from interrupting itself
        if (currentState == newState) return;

        // Play the animation
        animator.Play(newState);

        // Reassign the current state to the new one
        currentState = newState;
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
            // TODO: Update player sprite facing based off of movement
        }
    }
}
