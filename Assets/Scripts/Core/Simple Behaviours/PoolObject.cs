using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviours for pooled objects
/// </summary>

// Enum for pool object type
public enum PoolObjectType
{
    GENERIC_PROJECTILE_PLACEHOLDER,
    PROJECTILE_BULLET
}
public class PoolObject : MonoBehaviour
{
    [Header("Main Setting")]
    [SerializeField] internal PoolObjectType ObjectType;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        // Deactivate all pooled objects at the start of the game
        Deactivate();
    }

    // Activate pool object in hierarchy
    internal void Activate(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }
    internal PoolObject Activate(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
        return this;
    }

    // Deactivate pool object in hierarchy
    internal void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // Check if this pool object is active in hierarchy or not
    internal bool IsActive()
    {
        return gameObject.activeInHierarchy;
    }
}
