using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The projectile movement script (handles all projectile movements)
/// </summary>
[RequireComponent(typeof(MoveableScript))]
public class ProjectileMovementScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private ProjectileScript projectileScript;

    // Components
    private MoveableScript moveableComp;

    // Variables
    private Vector2 startingPosition;
    private Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        moveableComp = GetComponent<MoveableScript>();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Set starting position
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOutOfRange() && !projectileScript.projectileHitScript.HasHit())
        {
            // If projectile has travelled out of range without hitting anything, stop moving
            StopMoving();

            // TODO: Deactivate object using animation timestamps
            gameObject.SetActive(false);
        }
    }

    // Check if projectile had travelled its range distance
    internal bool IsOutOfRange()
    {
        return Vector2.Distance(startingPosition, transform.position) > projectileScript.range;
    }

    internal void SetSpeed(float velocity)
    {
        moveableComp.speed = velocity;
    }

    internal void SetDirection(Vector3 direction)
    {
        dir.x = direction.x;
        dir.y = direction.y;
        moveableComp.SetDirection(dir.normalized);
    }
    internal void SetDirection(Vector2 direction)
    {
        moveableComp.SetDirection(direction.normalized);
    }
    internal Vector3 GetDirection()
    {
        return moveableComp.GetDirectionWithVelocity().normalized;
    }

    internal void StopMoving()
    {
        moveableComp.StopMoving();
    }
}
