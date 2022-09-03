using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player movement script (handles all player movements)
/// </summary>
[RequireComponent(typeof(PlayerScript), typeof(Moveable))]
public class PlayerMovementScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private PlayerScript playerScript;

    // Components
    private Moveable moveableComp;

    // Variables
    internal Vector2 dir;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        Debug.Log("PlayerMovementScript starting");
        moveableComp = GetComponent<Moveable>();
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    // Move Player rigidbody in FixedUpdate
    private void FixedUpdate()
    {
        float MoveX = playerScript.playerInputScript.Input_MoveX;
        float MoveY = playerScript.playerInputScript.Input_MoveY;

        // Set direction vector for player movement
        dir = new Vector2(MoveX, MoveY);

        // Set moveable speed
        moveableComp.SetSpeed(playerScript.Speed);

        // Move using moveable
        moveableComp.SetDirection(dir);
    }
}
