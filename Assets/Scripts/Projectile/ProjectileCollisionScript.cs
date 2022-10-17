using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The projectile collision script (handles all projectile collision events)
/// </summary>
public class ProjectileCollisionScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private ProjectileScript projectileScript;

    // Components
    [SerializeField]
    internal Collider2D projectileCollider;

    // Variables
    [SerializeField] private string[] targetTags;

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Enable collider
        EnableCollider();
    }

    internal void EnableCollider()
    {
        projectileCollider.enabled = true;
    }

    internal void DisableCollider()
    {
        projectileCollider.enabled = false;
    }

    private void CollisionEnter(GameObject other)
    {
        if (!CheckTargetedTags(other)) return;

        // Call OnHit method
        projectileScript.projectileHitScript.OnHit(other);
    }
    private void Collision(GameObject other)
    {
        if (!CheckTargetedTags(other)) return;
    }
    private void CollisionExit(GameObject other)
    {
        if (!CheckTargetedTags(other)) return;
    }

    // Check if object collided with a desired tagged object
    private bool CheckTargetedTags(GameObject other)
    {
        foreach (string tag in targetTags)
        {
            if (other.gameObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    #region Boilerplate Methods
    // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEnter(collision.gameObject);
    }
    // OnCollisionExit2D is called when this collider2D/rigidbody2D has stopped touching another rigidbody2D/collider2D (2D physics only)
    private void OnCollisionExit2D(Collision2D collision)
    {
        Collision(collision.gameObject);
    }
    // OnCollisionStay2D is called once per frame for every collider2D/rigidbody2D that is touching rigidbody2D/collider2D (2D physics only)
    private void OnCollisionStay2D(Collision2D collision)
    {
        CollisionExit(collision.gameObject);
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionEnter(collision.gameObject);
    }
    // OnTriggerExit2D is called when the Collider2D other has stopped touching the trigger (2D physics only)
    private void OnTriggerExit2D(Collider2D collision)
    {
        Collision(collision.gameObject);
    }
    // OnTriggerStay2D is called once per frame for every Collider2D other that is touching the trigger (2D physics only)
    private void OnTriggerStay2D(Collider2D collision)
    {
        CollisionExit(collision.gameObject);
    }
    #endregion
}
