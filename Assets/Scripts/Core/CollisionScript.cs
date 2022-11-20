using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The collision script (handles all collision events)
/// </summary>
public class CollisionScript : MonoBehaviour
{
    // Components
    [SerializeField]
    internal Collider2D col;

    // Variables
    [SerializeField] private string[] targetTags;

    // Events
    [Header("Events")]
    internal UnityEvent OnCollisionEnter = new UnityEvent();
    internal UnityEvent OnCollision = new UnityEvent();
    internal UnityEvent OnCollisionExit = new UnityEvent();

    // Unity event that accepts a gameObject parameter
    // Mainly to access the other gameObject this object collides with
    internal UnityEvent<GameObject> OnCollisionEnterGO = new UnityEvent<GameObject>();
    internal UnityEvent<GameObject> OnCollisionGO = new UnityEvent<GameObject>();
    internal UnityEvent<GameObject> OnCollisionExitGO = new UnityEvent<GameObject>();

    // Collider settings
    internal void EnableCollider()
    {
        if (col)
            col.enabled = true;
    }
    internal void DisableCollider()
    {
        if (col)
            col.enabled = false;
    }

    // Collision Processes
    internal string CollisionEnter(GameObject other)
    {
        if (CheckTargetedTags(other) == null) return null;

        OnCollisionEnter?.Invoke();
        OnCollisionEnterGO?.Invoke(other);

        return other.tag;
    }
    internal string Collision(GameObject other)
    {
        if (CheckTargetedTags(other) == null) return null;

        OnCollision?.Invoke();
        OnCollisionGO?.Invoke(other);

        return other.tag;
    }
    private string CollisionExit(GameObject other, out string tag)
    {
        tag = other.tag;
        if (CheckTargetedTags(other) == null) return null;

        OnCollisionExit?.Invoke();
        OnCollisionExitGO?.Invoke(other);

        return other.tag;
    }
    internal string CollisionEnter(GameObject other, out string tag)
    {
        tag = other.tag;
        if (CheckTargetedTags(other) == null) return null;

        OnCollisionEnter?.Invoke();
        OnCollisionEnterGO?.Invoke(other);

        return other.tag;
    }
    internal string Collision(GameObject other, out string tag)
    {
        tag = other.tag;
        if (CheckTargetedTags(other) == null) return null;

        OnCollision?.Invoke();
        OnCollisionGO?.Invoke(other);

        return other.tag;
    }
    private string CollisionExit(GameObject other)
    {
        if (CheckTargetedTags(other) == null) return null;

        OnCollisionExit?.Invoke();
        OnCollisionExitGO?.Invoke(other);

        return other.tag;
    }
    // Collision Processes (with accepted target tags as arguments)
    internal string CollisionExit(GameObject other, string[] targetTags)
    {
        if (CheckTargetedTags(other, targetTags) == null) return null;

        OnCollisionExit?.Invoke();
        OnCollisionExitGO?.Invoke(other);

        return other.tag;
    }
    private string CollisionEnter(GameObject other, string[] targetTags)
    {
        if (CheckTargetedTags(other, targetTags) == null) return null;

        OnCollisionEnter?.Invoke();
        OnCollisionEnterGO?.Invoke(other);

        return other.tag;
    }
    private string Collision(GameObject other, string[] targetTags)
    {
        if (CheckTargetedTags(other, targetTags) == null) return null;

        OnCollision?.Invoke();
        OnCollisionGO?.Invoke(other);

        return other.tag;
    }

    // Check if object collided with a desired tagged object
    internal string CheckTargetedTags(GameObject other)
    {
        foreach (string tag in targetTags)
        {
            if (other.gameObject.CompareTag(tag))
            {
                return tag;
            }
        }
        return null;
    }
    internal string CheckTargetedTags(GameObject other, string[] targetTags)
    {
        foreach (string tag in targetTags)
        {
            if (other.gameObject.CompareTag(tag))
            {
                return tag;
            }
        }
        return null;
    }

    #region Boilerplate Methods
    // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionEnter(collision.GetContact(0).collider.gameObject);
    }
    // OnCollisionExit2D is called when this collider2D/rigidbody2D has stopped touching another rigidbody2D/collider2D (2D physics only)
    private void OnCollisionExit2D(Collision2D collision)
    {
        Collision(collision.collider.gameObject);
    }
    // OnCollisionStay2D is called once per frame for every collider2D/rigidbody2D that is touching rigidbody2D/collider2D (2D physics only)
    private void OnCollisionStay2D(Collision2D collision)
    {
        CollisionExit(collision.GetContact(0).collider.gameObject);
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
